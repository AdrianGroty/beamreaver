/*{
  "ISFVSN": "2.0",
  "VSN": "1.0",
  "DESCRIPTION": "X/Y oscillator pair",
  "CREDIT": "codex",
  "CATEGORIES": [
    "Generator"
  ],
  "INPUTS": [
    {
      "NAME": "xWave",
      "LABEL": "X Wave",
      "TYPE": "long",
      "VALUES": [
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7
      ],
      "LABELS": [
        "Sine",
        "Square",
        "Ramp u",
        "Ramp d",
        "Triangle",
        "Noise",
        "Lorenz",
        "R\u00f6ssler"
      ],
      "DEFAULT": 0
    },
    {
      "NAME": "xFreq",
      "LABEL": "X Freq",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.1
    },
    {
      "NAME": "xVol",
      "LABEL": "X Vol",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.7
    },
    {
      "NAME": "xPhase",
      "LABEL": "X Phase",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "xSync",
      "LABEL": "X Sync",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "yWave",
      "LABEL": "Y Wave",
      "TYPE": "long",
      "VALUES": [
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7
      ],
      "LABELS": [
        "Sine",
        "Square",
        "Ramp u",
        "Ramp d",
        "Triangle",
        "Noise",
        "Lorenz",
        "R\u00f6ssler"
      ],
      "DEFAULT": 0
    },
    {
      "NAME": "yFreq",
      "LABEL": "Y Freq",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.1
    },
    {
      "NAME": "yVol",
      "LABEL": "Y Vol",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.7
    },
    {
      "NAME": "yPhase",
      "LABEL": "Y Phase",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "ySync",
      "LABEL": "Y Sync",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "resolution",
      "LABEL": "Synthesis Resolution",
      "TYPE": "long",
      "VALUES": [
        0,
        1,
        2,
        3,
        4
      ],
      "LABELS": [
        "64 \u00d7 48",
        "128 \u00d7 96",
        "256 \u00d7 192",
        "512 \u00d7 384",
        "1024 \u00d7 768"
      ],
      "DEFAULT": 4
    },
    {
      "NAME": "transparent",
      "LABEL": "Transparent Background",
      "TYPE": "bool",
      "DEFAULT": true
    },
    {
      "NAME": "reset",
      "LABEL": "Reset Oscillators",
      "TYPE": "event"
    }
  ],
  "PASSES": [
    {
      "TARGET": "checkpoints",
      "PERSISTENT": true,
      "WIDTH": "16",
      "HEIGHT": "770"
    },
    {
      "TARGET": "signals",
      "WIDTH": "1024",
      "HEIGHT": "769"
    },
    {}
  ]
}*/

// Build with build.py. Standalone output is Cathodemer Oscillator Pair.fs.
// A row in checkpoints holds both axes' start states for that scan row.
// Row H is the completed-frame state. Storage is byte-exact RGBA8 float packing.
const float PI = 3.141592653589793;
const float TAU = 6.283185307179586;

vec2 gridFor(float setting) {
    if (setting < 0.5) return vec2(64.0,48.0);
    if (setting > 3.5) return vec2(1024.0,768.0);
    if (setting > 2.5) return vec2(512.0,384.0);
    if (setting > 1.5) return vec2(256.0,192.0);
    return vec2(128.0,96.0);
}
vec2 gridSize() { return gridFor(float(resolution)); }
float freq(float u) { return 0.0244140625*exp2(10.0*clamp(u,0.0,1.0)); }

vec4 packFloat(float value) {
    float a = abs(value);
    if (!(a > 0.0)) return vec4(0.0);
    // Normal float32 range used by these oscillators; no fixed signed range clamp.
    float e = floor(log2(a));
    float power = exp2(e);
    if (a < power) { e -= 1.0; power *= 0.5; }
    if (a >= power*2.0) { e += 1.0; power *= 2.0; }
    float m = floor((a/power-1.0)*8388608.0+0.5);
    if (m >= 8388608.0) { m=0.0; e+=1.0; }
    float exponent = e+127.0;
    float b0 = (value < 0.0 ? 128.0 : 0.0)+floor(exponent*0.5);
    float b1 = mod(exponent,2.0)*128.0+floor(m/65536.0);
    return vec4(b0,b1,mod(floor(m/256.0),256.0),mod(m,256.0))/255.0;
}
float unpackFloat(vec4 pixel) {
    vec4 b = floor(pixel*255.0+0.5);
    if (dot(b,vec4(1.0)) == 0.0) return 0.0;
    float e = mod(b.r,128.0)*2.0+floor(b.g/128.0)-127.0;
    float m = mod(b.g,128.0)*65536.0+b.b*256.0+b.a;
    return (b.r>=128.0 ? -1.0 : 1.0)*(1.0+m/8388608.0)*exp2(e);
}
vec4 readCheckpoint(float column, float row) {
    vec2 uv = (vec2(column,row)+0.5)/vec2(16.0,770.0);
    return IMG_NORM_PIXEL(checkpoints,uv);
}
float readValue(float axis, float field, float row) {
    return unpackFloat(readCheckpoint(axis*8.0+field,row));
}
vec3 readXYZ(float axis, float row) {
    return vec3(readValue(axis,0.0,row),readValue(axis,1.0,row),readValue(axis,2.0,row));
}
float selectedWave(float axis) { return float(axis<0.5 ? xWave : yWave); }
float selectedFreq(float axis) { return freq(axis<0.5 ? xFreq : yFreq); }
float axisLength(float axis) { vec2 size=gridSize(); return axis<0.5 ? size.x : size.y; }
float freshRows() { return min(gridSize().y,floor(gridSize().y*clamp(xSync,0.0,1.0))+1.0); }
float freshCols() { return min(gridSize().x,floor(gridSize().x*clamp(ySync,0.0,1.0))+1.0); }

vec3 euler(vec3 s, float wave, float f) {
    vec3 d;
    float dt = min(f,12.0)/512.0;
    if (wave < 6.5) {
        d = vec3(10.0*(s.y-s.x),s.x*(28.0-s.z)-s.y,s.x*s.y-2.666*s.z);
    } else {
        dt *= 2.91;
        d = vec3(-s.y-s.z,s.x+0.15*s.y,0.2+s.z*(s.x-10.0));
    }
    return s+dt*d;
}
vec3 integrate(vec3 state, float wave, float f, float count) {
    // Max count is the complete 1024*768 synthesis raster. No skipped samples.
    for (int i=0; i<786432; ++i) {
        if (float(i)>=count) break;
        state=euler(state,wave,f);
    }
    return state;
}
vec3 noiseAdvance(vec3 state, float f, float n, float count) {
    // state = held value, integer hold counter, PRNG state.
    // Independent deterministic PRNGs replace the unestablished native rand stream.
    float increment=floor(f*n);
    for (int i=0; i<786432; ++i) {
        if (float(i)>=count) break;
        if (state.y<=n) state.y+=increment;
        else {
            state.z=mod(state.z*75.0,65521.0);
            state.x=2.0*state.z/65520.0-1.0;
            state.y=increment;
        }
    }
    return state;
}

void checkpointPass() {
    float column=floor(gl_FragCoord.x);
    float row=floor(gl_FragCoord.y);
    vec2 size=gridSize();
    // Snapshot parameters alongside their states. This also supports hosts whose
    // multipass textures are exposed one frame later instead of immediately.
    if (row>768.5) {
        float v=0.0;
        if (column<0.5) v=xFreq;
        else if (column<1.5) v=xVol;
        else if (column<2.5) v=xPhase;
        else if (column<3.5) v=xSync;
        else if (column<4.5) v=yFreq;
        else if (column<5.5) v=yVol;
        else if (column<6.5) v=yPhase;
        else if (column<7.5) v=ySync;
        else if (column<8.5) {
            gl_FragColor=vec4(float(xWave),float(yWave),float(resolution),173.0)/255.0;
            return;
        } else if (column<9.5) v=transparent ? 1.0 : 0.0;
        gl_FragColor=packFloat(v);
        return;
    }
    if (column>14.5) {
        // Byte metadata: wave X, wave Y, resolution enum, initialized signature.
        gl_FragColor=vec4(float(xWave),float(yWave),float(resolution),173.0)/255.0;
        return;
    }
    if (row>size.y) { gl_FragColor=vec4(0.0); return; }
    float axis=step(8.0,column);
    float field=column-axis*8.0;
    if (field>6.5) { gl_FragColor=vec4(0.0); return; }
    vec4 meta=floor(readCheckpoint(15.0,0.0)*255.0+0.5);
    bool fresh=FRAMEINDEX==0 || meta.a!=173.0 || meta.b!=float(resolution);
    float oldWave=axis<0.5 ? meta.r : meta.g;
    float wave=selectedWave(axis);
    float f=selectedFreq(axis);
    float n=axisLength(axis);
    float count=axis<0.5 ? min(row,freshRows())*size.x : row*freshCols();
    bool newChaos=wave>=6.0 && oldWave!=wave;
    float value=0.0;
    if (field<2.5) {
        vec3 state=fresh ? vec3(1.0) : readXYZ(axis,size.y);
        if (newChaos || reset) state=vec3(1.0);
        if (wave>=6.0) state=integrate(state,wave,f,count);
        value=field<0.5 ? state.x : (field<1.5 ? state.y : state.z);
    } else if (field<3.5) {
        float cursor=fresh || reset ? 0.0 : readValue(axis,3.0,size.y);
        // Periodic cursor can jump directly; float roundoff can differ from C++ p+=f.
        value=mod(cursor+count*f,n);
    } else {
        vec3 state=fresh ? vec3(0.0,0.0,axis<0.5 ? 127.0 : 8191.0)
                        : vec3(readValue(axis,4.0,size.y),readValue(axis,5.0,size.y),readValue(axis,6.0,size.y));
        if (wave>4.5 && wave<5.5) state=noiseAdvance(state,f,n,count);
        value=field<4.5 ? state.x : (field<5.5 ? state.y : state.z);
    }
    gl_FragColor=packFloat(value);
}

float tableWave(float wave, float i, float n) {
    float halfSize=floor(n*0.5);
    if (wave<0.5) return sin(TAU*i/n);
    if (wave<1.5) return i<halfSize ? -0.9999 : 0.9999;
    if (wave<3.5) {
        float ramp=i/halfSize-(i<halfSize ? 0.0 : 2.0);
        return wave<2.5 ? ramp : -ramp;
    }
    return i<halfSize ? 2.0*i/halfSize-1.0 : 1.0-2.0*(i-halfSize)/halfSize;
}
float sampleAxis(float axis, float col, float row) {
    vec4 meta=floor(readCheckpoint(8.0,769.0)*255.0+0.5);
    vec2 size=gridFor(meta.b);
    float wave=axis<0.5 ? meta.r : meta.g;
    float offset=axis*4.0;
    float f=freq(unpackFloat(readCheckpoint(offset,769.0)));
    float n=axis<0.5 ? size.x : size.y;
    float phase=clamp(unpackFloat(readCheckpoint(offset+2.0,769.0)),0.0,1.0);
    float volume=clamp(unpackFloat(readCheckpoint(offset+1.0,769.0)),0.0,1.0);
    float sync=clamp(unpackFloat(readCheckpoint(offset+3.0,769.0)),0.0,1.0);
    float rx=min(size.y,floor(size.y*sync)+1.0);
    float cy=min(size.x,floor(size.x*sync)+1.0);
    float checkpointRow=axis<0.5 ? min(row,rx-1.0) : row;
    float count=axis<0.5 ? col+1.0 : min(col+1.0,cy);
    if (wave>=6.0) {
        vec3 state=readXYZ(axis,checkpointRow);
        // At most one synthesis row, even at full sync.
        for (int i=0; i<1024; ++i) {
            if (float(i)>=count) break;
            state=euler(state,wave,f);
        }
        float gain=wave<6.5 ? (phase<0.5 ? 0.05107 : 0.03679)
                            : (phase<0.5 ? 0.05757 : 0.06028);
        return volume*state.x*gain;
    }
    if (wave>4.5) {
        vec3 noiseState=vec3(readValue(axis,4.0,checkpointRow),readValue(axis,5.0,checkpointRow),readValue(axis,6.0,checkpointRow));
        return volume*noiseAdvance(noiseState,f,n,count).x;
    }
    float cursor=readValue(axis,3.0,checkpointRow);
    cursor=mod(cursor+count*f,n);
    float index=mod(floor(cursor)+floor(phase*n),n);
    return volume*tableWave(wave,index,n);
}

void signalPass() {
    vec2 pos=floor(gl_FragCoord.xy);
    vec4 meta=floor(readCheckpoint(8.0,769.0)*255.0+0.5);
    if (meta.a!=173.0) { gl_FragColor=vec4(0.0); return; }
    vec2 size=gridFor(meta.b);
    if (pos.y>767.5) {
        float clear=unpackFloat(readCheckpoint(9.0,769.0));
        gl_FragColor=vec4(meta.b,clear,0.0,173.0)/255.0;
        return;
    }
    if (pos.x>=size.x || pos.y>=size.y) { gl_FragColor=vec4(0.0); return; }
    float value=clamp(sampleAxis(0.0,pos.x,pos.y)+sampleAxis(1.0,pos.x,pos.y),0.0,1.0);
    gl_FragColor=vec4(value,value,value,1.0);
}
void displayPass() {
    vec2 metaUV=vec2(0.5,768.5)/vec2(1024.0,769.0);
    vec4 meta=floor(IMG_NORM_PIXEL(signals,metaUV)*255.0+0.5);
    if (meta.a!=173.0) { gl_FragColor=vec4(0.0); return; }
    vec2 size=gridFor(meta.r);
    vec2 pos=floor(vec2(isf_FragNormCoord.x,1.0-isf_FragNormCoord.y)*size);
    pos=clamp(pos,vec2(0.0),size-1.0);
    vec2 uv=(pos+0.5)/vec2(1024.0,769.0);
    float value=IMG_NORM_PIXEL(signals,uv).r;
    gl_FragColor=meta.g>0.5 ? (value>0.0 ? vec4(1.0,1.0,1.0,value) : vec4(0.0))
                            : vec4(vec3(value),1.0);
}
void main() {
    if (PASSINDEX==0) checkpointPass();
    else if (PASSINDEX==1) signalPass();
    else displayPass();
}
