/*{
  "ISFVSN": "2.0",
  "VSN": "1.4-geometry.2",
  "DESCRIPTION": "X/Y function generator",
  "CREDIT": "codex",
  "CATEGORIES": [
    "Generator",
    "Geometry"
  ],
  "INPUTS": [
    {
      "NAME": "combination",
      "LABEL": "Combination",
      "TYPE": "long",
      "VALUES": [
        0,
        1,
        2,
        3
      ],
      "LABELS": [
        "f(x)",
        "f(y)",
        "f(x,y)",
        "f(y,x)"
      ],
      "DEFAULT": 2
    },
    {
      "NAME": "xFunction",
      "LABEL": "X Function",
      "TYPE": "long",
      "VALUES": [
        0,
        1,
        2,
        3,
        4,
        5
      ],
      "LABELS": [
        "linear",
        "sin",
        "cos",
        "sqrt",
        "log",
        "?"
      ],
      "DEFAULT": 1
    },
    {
      "NAME": "yFunction",
      "LABEL": "Y Function",
      "TYPE": "long",
      "VALUES": [
        0,
        1,
        2,
        3,
        4,
        5
      ],
      "LABELS": [
        "linear",
        "sin",
        "cos",
        "sqrt",
        "log",
        "?"
      ],
      "DEFAULT": 2
    },
    {
      "NAME": "order",
      "LABEL": "Order (sampling density)",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 99,
      "DEFAULT": 3
    },
    {
      "NAME": "depth",
      "LABEL": "Depth",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 98,
      "DEFAULT": 6
    },
    {
      "NAME": "increment",
      "LABEL": "Recursion Increment",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 98,
      "DEFAULT": 8
    },
    {
      "NAME": "rotationSpeed",
      "LABEL": "Rotation Speed",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "rotationInc",
      "LABEL": "Rotation Inc",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "rotationPhase",
      "LABEL": "Rotation Phase (0..1 = one cycle)",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "radiusAmount",
      "LABEL": "Radius Amount",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "radiusSpeed",
      "LABEL": "Radius Speed",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0.0
    },
    {
      "NAME": "radiusInc",
      "LABEL": "Radius Inc",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "radiusPhase",
      "LABEL": "Radius Phase (0..1 = one cycle)",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "geometryScale",
      "LABEL": "Geometry Scale",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 4,
      "DEFAULT": 1
    },
    {
      "NAME": "originX",
      "LABEL": "Origin X",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.5
    },
    {
      "NAME": "originY",
      "LABEL": "Origin Y",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.5
    },
    {
      "NAME": "bending",
      "LABEL": "Bend On",
      "TYPE": "bool",
      "DEFAULT": false
    },
    {
      "NAME": "horizontalBend",
      "LABEL": "Horizontal Bend",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "verticalBend",
      "LABEL": "Vertical Bend",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "bendMode",
      "LABEL": "Bend Mode",
      "TYPE": "long",
      "VALUES": [
        0,
        1,
        2,
        3
      ],
      "LABELS": [
        "Square",
        "Lens",
        "Sphere",
        "Twist"
      ],
      "DEFAULT": 0
    },
    {
      "NAME": "bendFunction",
      "LABEL": "Bend Function",
      "TYPE": "long",
      "VALUES": [
        0,
        1
      ],
      "LABELS": [
        "Light",
        "Heavy"
      ],
      "DEFAULT": 0
    },
    {
      "NAME": "viewZoom",
      "LABEL": "View Zoom",
      "TYPE": "float",
      "MIN": 0.5,
      "MAX": 2,
      "DEFAULT": 1
    },
    {
      "NAME": "pointSize",
      "LABEL": "Point Size (reference pixels)",
      "TYPE": "float",
      "MIN": 0.5,
      "MAX": 4,
      "DEFAULT": 1.5
    },
    {
      "NAME": "connectSamples",
      "LABEL": "Connect Samples",
      "TYPE": "bool",
      "DEFAULT": false
    },
    {
      "NAME": "resetPhases",
      "LABEL": "Reset Accumulated Phases",
      "TYPE": "event"
    }
  ],
  "PASSES": [
    {
      "TARGET": "phaseState",
      "PERSISTENT": true,
      "FLOAT": true,
      "WIDTH": "1",
      "HEIGHT": "1"
    },
    {
      "TARGET": "curvePoints",
      "FLOAT": true,
      "WIDTH": "1024",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds1",
      "FLOAT": true,
      "WIDTH": "32",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds2",
      "FLOAT": true,
      "WIDTH": "16",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds3",
      "FLOAT": true,
      "WIDTH": "8",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds4",
      "FLOAT": true,
      "WIDTH": "4",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds5",
      "FLOAT": true,
      "WIDTH": "2",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds6",
      "FLOAT": true,
      "WIDTH": "1",
      "HEIGHT": "98"
    },
    {
      "TARGET": "agGeometryBounds",
      "WIDTH": "1",
      "HEIGHT": "1",
      "FLOAT": true
    },
    {}
  ]
}*/

#define AF_TAU 6.283185307179586
#define AF_PI 3.141592653589793
#define AF_RADIUS_STEP 1.2566370964050293
#define AF_REFERENCE vec2(960.0,640.0)

float sampleCount() { return floor(9.6*clamp(floor(order),1.0,100.0)); }
float layerCount() { return clamp(floor(depth),1.0,98.0); }
float truncZero(float v) { return sign(v)*floor(abs(v)); }
vec2 truncPoint(vec2 v) { return sign(v)*floor(abs(v)); }

float rotationAdjust(float a) {
    if (a>=AF_TAU) return abs(a-AF_TAU);
    if (a<0.0) return AF_TAU-abs(a);
    return a;
}

// Negative radius arguments are reflected, matching the recovered branch.
float radiusAdjust(float a) {
    if (a>=AF_TAU) return abs(a-AF_TAU);
    if (a<0.0) return AF_TAU-a;
    return a;
}

float signedCycle(float a) { return sign(a)*mod(abs(a),AF_TAU); }

// .r = accumulated rotation cycles; .g = radius traversal-start phase.
// The time-integrated clock avoids TIME*speed phase jumps when a knob changes.
void clockPass() {
    vec4 old=IMG_NORM_PIXEL(phaseState,vec2(0.5));
    if (FRAMEINDEX==0 || resetPhases) old=vec4(0.0);
    float dt=(FRAMEINDEX==0 || resetPhases) ? 0.0 : max(TIMEDELTA,0.0);
    float rot=mod(old.r+rotationSpeed*dt,1.0);
    float radius=old.g;
    radius=signedCycle(radius+sampleCount()*layerCount()*AF_RADIUS_STEP*radiusSpeed*25.0*dt);
    gl_FragColor=vec4(rot,radius,0.0,1.0);
}

bool functionValid(int mode,float u) {
    if (mode==4) return u>=0.0;
    if (mode==5) return u>0.0;
    if (mode==3) return abs(cos(u))>0.000001;
    return true;
}

float functionValue(int mode,float u,float phi) {
    // Integer i/N == 0 for every i<N: preserve the original linear quirk.
    if (mode==0) return phi;
    if (mode==1) return sin(u);
    if (mode==2) return cos(u);
    if (mode==3) return tan(u);
    if (mode==4) return sqrt(max(u,0.0));
    return log(max(u,0.00000001));
}

vec4 generatedPoint(float i,float layer) {
    float n=sampleCount();
    if (i>=n || layer>=layerCount()) return vec4(0.0);
    vec4 clock=IMG_NORM_PIXEL(phaseState,vec2(0.5));
    float phi=rotationAdjust(AF_TAU*mod(clock.r+rotationPhase,1.0)+layer*rotationInc);
    float u=phi+i/(AF_PI*n);
    bool vx=functionValid(xFunction,u),vy=functionValid(yFunction,u);
    if ((combination==0 && !vx) || (combination==1 && !vy) || (combination>=2 && (!vx || !vy))) return vec4(0.0);
    float raw=clock.g+AF_TAU*mod(radiusPhase,1.0)+(layer*n+i+1.0)*AF_RADIUS_STEP*radiusSpeed+i*radiusInc/18.0;
    float modulation=radiusAmount*(1.0+0.5*sin(radiusAdjust(raw)));
    float scale=1.0+layer*2.0*increment*(geometryScale+0.5);
    float radius=scale*(1.0+modulation);
    float fx=functionValue(xFunction,u,phi),fy=functionValue(yFunction,u,phi);
    vec2 p;
    if (combination==0) p=vec2(i*floor(960.0/n),float(int(640.0*(1.0-originY)))+radius*fx);
    else if (combination==1) p=vec2(float(int(960.0*originX))+radius*fy,i*floor(640.0/n));
    else if (combination==2) p=vec2(float(int(960.0*originX))+radius*fy,float(int(640.0*(1.0-originY)))+radius*fx);
    else p=vec2(float(int(960.0*originX))+radius*fx,float(int(640.0*(1.0-originY)))+radius*fy);
    // Invalid/singular coordinates have no drawable sample. They never bridge
    // a discontinuity when Connect Samples is enabled.
    if (!(abs(p.x)<1.0e9 && abs(p.y)<1.0e9)) return vec4(0.0);
    return vec4(truncPoint(p),1.0,1.0);
}

vec4 pointAt(float i,float layer) {
    if (i<0.0 || i>=1024.0) return vec4(0.0);
    return IMG_NORM_PIXEL(curvePoints,vec2((i+0.5)/1024.0,(layer+0.5)/98.0));
}

bool segmentValid(vec4 a,vec4 b) {
    return a.z>0.5 && b.z>0.5 && abs(a.x-b.x)<240.0 && abs(a.y-b.y)<240.0;
}

vec4 emptyBounds() { return vec4(16384.0,16384.0,-16384.0,-16384.0); }

vec4 fetchedLeafBounds(vec4 p,vec4 previous) {
    if (p.z<0.5) return emptyBounds();
    if (connectSamples) {
        if (segmentValid(p,previous)) return vec4(min(p.xy,previous.xy)-pointSize*0.5,max(p.xy,previous.xy)+pointSize*0.5);
        // The first/isolated sample is a dot, so collapsed paths stay visible.
        return vec4(p.xy,p.xy+pointSize);
    }
    return vec4(p.xy,p.xy+pointSize);
}

// Bounds construction fetches each endpoint once.
vec4 leafBounds(float i,float layer) {
    vec4 p=pointAt(i,layer);
    vec4 previous=vec4(0.0);
    if (connectSamples && p.z>=0.5) previous=pointAt(i-1.0,layer);
    return fetchedLeafBounds(p,previous);
}

vec4 boundsAt(int level,float index,float layer) {
    if (level==0) return leafBounds(index,layer);
    float y=(layer+0.5)/98.0;
    if (level==1) return IMG_NORM_PIXEL(bounds1,vec2((index+0.5)/32.0,y));
    if (level==2) return IMG_NORM_PIXEL(bounds2,vec2((index+0.5)/16.0,y));
    if (level==3) return IMG_NORM_PIXEL(bounds3,vec2((index+0.5)/8.0,y));
    if (level==4) return IMG_NORM_PIXEL(bounds4,vec2((index+0.5)/4.0,y));
    if (level==5) return IMG_NORM_PIXEL(bounds5,vec2((index+0.5)/2.0,y));
    return IMG_NORM_PIXEL(bounds6,vec2(index+0.5,y));
}

void boundsPass() {
    int level=PASSINDEX-1;
    float index=floor(gl_FragCoord.x),layer=floor(gl_FragCoord.y);
    if (level==1) {
        vec4 box=emptyBounds();
        for (int k=0;k<32;k++) {
            vec4 item=leafBounds(index*32.0+float(k),layer);
            box=vec4(min(box.xy,item.xy),max(box.zw,item.zw));
        }
        gl_FragColor=box;return;
    }
    vec4 a=boundsAt(level-1,index*2.0,layer),b=boundsAt(level-1,index*2.0+1.0,layer);
    gl_FragColor=vec4(min(a.xy,b.xy),max(a.zw,b.zw));
}

float boxDistance(vec2 p,vec2 a,vec2 b) {
    vec2 d=abs(p-(a+b)*0.5)-(b-a)*0.5;
    return length(max(d,vec2(0.0)))+min(max(d.x,d.y),0.0);
}

float fetchedLeafCoverage(vec2 p,vec4 a,vec4 b,float aa) {
    if (a.z<0.5) return 0.0;
    float distanceToShape=boxDistance(p,a.xy,a.xy+pointSize);
    if (connectSamples) {
        if (segmentValid(a,b)) {
            vec2 delta=a.xy-b.xy;
            float t=clamp(dot(p-b.xy,delta)/max(dot(delta,delta),0.000001),0.0,1.0);
            distanceToShape=length(p-(b.xy+t*delta))-pointSize*0.5;
        }
    }
    return 1.0-smoothstep(-aa,aa,distanceToShape);
}

// Stackless complete binary tree: a rejected subtree is skipped in one step.
// No dynamic sampler indexing, recursion, or custom vertex pipeline is needed.
float curveCoverage(vec2 p,float aa) {
    float coverage=0.0;
    for (int layer=0;layer<98;layer++) {
        if (float(layer)>=layerCount()) break;
        int node=1;
        int treeDepth=0;
        float width=1.0;
        for (int visit=0;visit<63;visit++) {
            // Maintain the level as traversal descends and ascends.
            float index=float(node)-width;
            vec4 box=boundsAt(6-treeDepth,index,float(layer));
            bool hit=box.x<=box.z && box.y<=box.w && p.x>=box.x-aa && p.y>=box.y-aa && p.x<=box.z+aa && p.y<=box.w+aa;
            if (hit && node<32) {
                node*=2;treeDepth++;width*=2.0;continue;
            }
            if (hit) {
                for (int k=0;k<32;k++) {
                    float sampleIndex=float(node-32)*32.0+float(k);
                    vec4 a=pointAt(sampleIndex,float(layer));
                    if (a.z<0.5) continue;
                    vec4 b=vec4(0.0);
                    if (connectSamples) b=pointAt(sampleIndex-1.0,float(layer));
                    vec4 leaf=fetchedLeafBounds(a,b);
                    if (p.x>=leaf.x-aa && p.y>=leaf.y-aa && p.x<=leaf.z+aa && p.y<=leaf.w+aa)
                        coverage=max(coverage,fetchedLeafCoverage(p,a,b,aa));
                    if (coverage>=0.9999) return coverage;
                }
            }
            for (int ascend=0;ascend<6;ascend++) {
                if (node<=1 || mod(float(node),2.0)<0.5) break;
                node/=2;treeDepth--;width*=0.5;
            }
            if (node<=1) break;
            node++;
        }
    }
    return coverage;
}

vec4 agCalculateBounds(){vec4 box=vec4(1.e20,1.e20,-1.e20,-1.e20);for(int layer=0;layer<98;layer++){if(float(layer)>=layerCount())break;vec4 b=boundsAt(6,0.0,float(layer));if(b.x<=b.z && b.y<=b.w)box=vec4(min(box.xy,b.xy),max(box.zw,b.zw));}return box+vec4(-pointSize-4.0,-pointSize-4.0,pointSize+4.0,pointSize+4.0);}

#define AG_SOURCE_COVERAGE curveCoverage
// Shared Arabesque Geometry panel. Reference coordinates have Y down.
// Recovered from dispatcher 0x460a78..0x460b15 and point helper 0x459470.
// Continuous coverage replaces the native integer scatter rasterization.
vec2 agBend(vec2 p) {
    vec2 size=vec2(960.0,640.0),center=size*.5;
    vec2 amount=vec2(horizontalBend,verticalBend);
    float gain=bendFunction==0?1.0:5.0;
    if(bendMode==0) return p+(p-center)*amount*(bendFunction==0?1.0:10.0);
    if(bendMode==1) return p+sin((p-center)/size*1.570796326794897)*size*.5*amount*gain;
    vec2 wave=vec2(bendMode==3?cos(p.y/size.y*3.141592653589793):sin(p.y/size.y*3.141592653589793),sin(p.x/size.x*3.141592653589793));
    return p+wave*size*.5*amount*gain;
}
// Columns of the analytic forward Jacobian.
mat2 agJacobian(vec2 p) {
    vec2 size=vec2(960.0,640.0),amount=vec2(horizontalBend,verticalBend);
    float gain=bendFunction==0?1.0:5.0;
    if(bendMode==0){vec2 d=1.0+amount*(bendFunction==0?1.0:10.0);return mat2(d.x,0.0,0.0,d.y);}
    if(bendMode==1){vec2 d=1.0+cos((p-size*.5)/size*1.570796326794897)*amount*(.7853981633974483*gain);return mat2(d.x,0.0,0.0,d.y);}
    float dxdy=(bendMode==3?-sin(p.y/640.0*3.141592653589793):cos(p.y/640.0*3.141592653589793))*amount.x*(.75*3.141592653589793*gain);
    float dydx=cos(p.x/960.0*3.141592653589793)*amount.y*(3.141592653589793/3.0*gain);
    return mat2(1.0,dydx,dxdy,1.0);
}
vec2 agSolve(mat2 j,vec2 v) {
    float det=j[0].x*j[1].y-j[1].x*j[0].y;
    if(abs(det)<.00001)det=det<0.0?-.00001:.00001;
    return vec2(j[1].y*v.x-j[1].x*v.y,-j[0].y*v.x+j[0].x*v.y)/det;
}
float agCoverageAt(vec2 p,float aa){return AG_SOURCE_COVERAGE(p,aa);}
vec4 agSourceBounds(float aa){
    vec2 uv=vec2(.5);
    vec4 box=IMG_NORM_PIXEL(agGeometryBounds,uv);
    return box+vec4(-aa*100.0,-aa*100.0,aa*100.0,aa*100.0);
}
vec2 agWaveRange(float lo,float hi,bool cosine){
    if(cosine){lo+=1.570796326794897;hi+=1.570796326794897;}
    vec2 r=vec2(min(sin(lo),sin(hi)),max(sin(lo),sin(hi)));
    if(ceil((lo-1.570796326794897)/6.283185307179586)<=floor((hi-1.570796326794897)/6.283185307179586))r.y=1.0;
    if(ceil((lo+1.570796326794897)/6.283185307179586)<=floor((hi+1.570796326794897)/6.283185307179586))r.x=-1.0;
    return r;
}
// Eliminate y from the coupled Sphere/Twist equations. Every inverse point
// satisfies y=target.y-B*sin(pi*x/960), leaving a bounded scalar equation.
float agCoupledValue(float x,vec2 target,vec2 amount){
    float y=target.y-amount.y*sin(x*.003272492347489368);
    float angle=y*.004908738521234052;
    return x+amount.x*(bendMode==3?cos(angle):sin(angle))-target.x;
}
vec2 agCoupledValueDerivative(float x,vec2 target,vec2 amount){
    float ax=x*.003272492347489368;
    float y=target.y-amount.y*sin(ax);
    float ay=y*.004908738521234052;
    float wave=bendMode==3?cos(ay):sin(ay);
    float derivative=bendMode==3?-sin(ay):cos(ay);
    return vec2(x+amount.x*wave-target.x,1.0-amount.x*amount.y*.00001606380924656471*derivative*cos(ax));
}
float agCoupledCoverage(vec2 target,float aa){
    vec2 amount=vec2(horizontalBend*2400.0,verticalBend*1600.0);
    if(bendFunction==0)amount*=.2;
    // The original search runs 25 independent two-dimensional Newton solves.
    // Scan the exact bounded scalar domain, then refine only sign brackets.
    // Cost follows bend strength; the search does not evaluate source geometry
    // for failed seeds or rediscover the same root from different seeds.
    vec4 box=agSourceBounds(aa);
    if(box.x>box.z || box.y>box.w)return 0.0;
    vec2 xr=agWaveRange(box.y*.004908738521234052,box.w*.004908738521234052,bendMode==3)*amount.x;
    vec2 yr=agWaveRange(box.x*.003272492347489368,box.z*.003272492347489368,false)*amount.y;
    if(target.x<box.x+min(xr.x,xr.y) || target.x>box.z+max(xr.x,xr.y) || target.y<box.y+min(yr.x,yr.y) || target.y>box.w+max(yr.x,yr.y))return 0.0;
    float start=max(target.x-abs(amount.x)-.001,box.x),finish=min(target.x+abs(amount.x)+.001,box.z);
    if(start>=finish)return 0.0;
    int intervals=int(clamp(ceil((finish-start)/20.0),16.0,256.0));
    float low=start;
    float step=(finish-start)/float(intervals);
    float fLow=agCoupledValue(low,target,amount),result=0.0;
    for(int i=0;i<256;i++){
        if(i>=intervals)break;
        float high=start+float(i+1)*step;
        float fHigh=agCoupledValue(high,target,amount);
        if(fLow*fHigh<=0.0){
            float left=low,right=high,fl=fLow;
            float x=(left+right)*.5;
            for(int it=0;it<18;it++){
                vec2 fd=agCoupledValueDerivative(x,target,amount);
                if(abs(fd.x)<.0001)break;
                if(fl*fd.x<=0.0)right=x;else{left=x;fl=fd.x;}
                float next=x-fd.x/(abs(fd.y)>.00001?fd.y:.00001);
                x=(next>left && next<right)?next:(left+right)*.5;
            }
            if(abs(agCoupledValue(x,target,amount))<.01){
                vec2 p=vec2(x,target.y-amount.y*sin(x*.003272492347489368));
                if(p.y<box.y || p.y>box.w){low=high;fLow=fHigh;continue;}
                mat2 j=agJacobian(p);
                float stretch=max(length(agSolve(j,vec2(1.0,0.0))),length(agSolve(j,vec2(0.0,1.0))));
                result=max(result,agCoverageAt(p,aa*clamp(stretch,.01,100.0)));
                if(result>=1.0)return result;
            }
        }
        low=high;fLow=fHigh;
    }
    return result;
}
float agGeometryCoverage(vec2 target,float aa) {
    if(!bending || (horizontalBend==0.0 && verticalBend==0.0))return agCoverageAt(target,aa);
    if(bendMode==0){
        vec2 d=1.0+vec2(horizontalBend,verticalBend)*(bendFunction==0?1.0:10.0);
        if(abs(d.x)<.0001)d.x=d.x<0.0?-.0001:.0001;
        if(abs(d.y)<.0001)d.y=d.y<0.0?-.0001:.0001;
        return agCoverageAt((target-vec2(480.0,320.0))/d+vec2(480.0,320.0),aa/min(abs(d.x),abs(d.y)));
    }
    // Search multiple inverse branches: strong signed bends can fold strokes.
    // Coverage remains analytic; the extra 1x1 bounds cache never resamples
    // the image or updates clocks. Existing Lens/unique-map behavior stays.
    vec2 amplitude=abs(vec2(horizontalBend,verticalBend))*vec2(480.0,320.0)*(bendFunction==0?1.0:5.0);
    float gain=bendFunction==0?1.0:5.0;
    bool unique=bendMode==1 ? max(abs(horizontalBend),abs(verticalBend))*.7853981633974483*gain<.99 : abs(horizontalBend*verticalBend)*2.46740110027234*gain*gain<.99;
    if(bendMode>=2 && !unique)return agCoupledCoverage(target,aa);
    vec2 roots[25];int count=0;float result=0.0;
    for(int sy=0;sy<5;sy++)for(int sx=0;sx<5;sx++){
        vec2 p=target+amplitude*(vec2(float(sx),float(sy))*.5-1.0);
        if(sx==0 && sy==0)p=target;
        for(int it=0;it<24;it++){
            vec2 error=agBend(p)-target;
            if(dot(error,error)<.000001)break;
            vec2 step=agSolve(agJacobian(p),error);
            // Damping keeps near-singular Newton steps inside the search region.
            float factor=max(1.0,max(abs(step.x)/(amplitude.x+1.0),abs(step.y)/(amplitude.y+1.0)));
            p-=step/factor;
        }
        if(length(agBend(p)-target)>.01)continue;
        bool duplicate=false;
        for(int k=0;k<25;k++){if(k>=count)break;if(length(p-roots[k])<.05)duplicate=true;}
        if(duplicate)continue;
        roots[count]=p;count++;
        mat2 j=agJacobian(p);
        float stretch=max(length(agSolve(j,vec2(1.0,0.0))),length(agSolve(j,vec2(0.0,1.0))));
        result=max(result,agCoverageAt(p,aa*clamp(stretch,.01,100.0)));
        if(unique)return result;
        if(result>=1.0)return result;
    }
    return result;
}

void outputPass() {
    float fit=min(RENDERSIZE.x/960.0,RENDERSIZE.y/640.0);
    vec2 windowPoint=vec2(gl_FragCoord.x,RENDERSIZE.y-gl_FragCoord.y);
    vec2 canvasPoint=(windowPoint-RENDERSIZE*0.5)/fit+AF_REFERENCE*0.5;
    vec2 p=(canvasPoint-AF_REFERENCE*0.5)/viewZoom+AF_REFERENCE*0.5;
    float aa=0.55/(fit*viewZoom);
    float coverage=agGeometryCoverage(p,aa);
    // Straight-alpha output. Remove sub-byte coverage so RGBA8 hosts cannot
    // quantize alpha to zero while retaining RGB on an otherwise clear pixel.
    gl_FragColor=coverage>=1.0/255.0 ? vec4(vec3(1.0),coverage) : vec4(0.0);
}

void agOriginalMain() {
    if (PASSINDEX==0) clockPass();
    else if (PASSINDEX==1) gl_FragColor=generatedPoint(floor(gl_FragCoord.x),floor(gl_FragCoord.y));
    else if (PASSINDEX<=7) boundsPass();
    else outputPass();
}

void main(){if(PASSINDEX==8){gl_FragColor=agCalculateBounds();return;}agOriginalMain();}
