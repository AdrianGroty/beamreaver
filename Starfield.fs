/*{
  "ISFVSN": "2",
  "DESCRIPTION": "starfield",
  "CREDIT": "codex",
  "CATEGORIES": [
    "Generator"
  ],
  "INPUTS": [
    {
      "NAME": "mode",
      "LABEL": "Mode",
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
        "Off",
        "X",
        "Y",
        "Z",
        "X+Y",
        "X-Y"
      ],
      "DEFAULT": 3
    },
    {
      "NAME": "originX",
      "LABEL": "Origin X (Z mode)",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.5
    },
    {
      "NAME": "originY",
      "LABEL": "Origin Y (Z mode)",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.5
    },
    {
      "NAME": "speed",
      "LABEL": "Speed",
      "TYPE": "float",
      "MIN": -5,
      "MAX": 5,
      "DEFAULT": 0.75
    },
    {
      "NAME": "amount",
      "LABEL": "Amount",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.75
    },
    {
      "NAME": "depth",
      "LABEL": "Depth",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.5
    },
    {
      "NAME": "sorting",
      "LABEL": "Sorting",
      "TYPE": "bool",
      "DEFAULT": false
    },
    {
      "NAME": "volume",
      "LABEL": "Volume",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 1
    },
    {
      "NAME": "pointSize",
      "LABEL": "Point Size (reference pixels)",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 8,
      "DEFAULT": 1
    },
    {
      "NAME": "rate",
      "LABEL": "Reference redraws / second",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 120,
      "DEFAULT": 25
    },
    {
      "NAME": "seed",
      "LABEL": "Seed",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1000,
      "DEFAULT": 1
    },
    {
      "NAME": "reset",
      "LABEL": "Reset",
      "TYPE": "event"
    }
  ],
  "PASSES": [
    {
      "TARGET": "stars",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "PERSISTENT": true,
      "FLOAT": true
    },
    {
      "TARGET": "scratch",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "stars",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true,
      "PERSISTENT": true
    },
    {
      "TARGET": "scratch",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "stars",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true,
      "PERSISTENT": true
    },
    {
      "TARGET": "scratch",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "stars",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true,
      "PERSISTENT": true
    },
    {
      "TARGET": "scratch",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "stars",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true,
      "PERSISTENT": true
    },
    {
      "TARGET": "scratch",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "stars",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true,
      "PERSISTENT": true
    },
    {
      "TARGET": "scratch",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "stars",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true,
      "PERSISTENT": true
    },
    {
      "TARGET": "scratch",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "stars",
      "WIDTH": "4097",
      "HEIGHT": "2",
      "FLOAT": true,
      "PERSISTENT": true
    },
    {
      "TARGET": "projected",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "pixelScratch",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "projected",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "pixelScratch",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "projected",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "pixelScratch",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "projected",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "pixelScratch",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "projected",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "pixelScratch",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "projected",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "pixelScratch",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "projected",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "pixelScratch",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {
      "TARGET": "projected",
      "WIDTH": "4096",
      "HEIGHT": "2",
      "FLOAT": true
    },
    {}
  ]
}*/
// Reference raster 960 x 640. Particle records: x, y, z, pre-update fade.
float tr(float x) { return sign(x)*floor(abs(x)); }
// Keep every hash input small: the host frame counter can already be large
// when this shader loads. Large sine arguments collapse on some GPUs.
float rnd(float n) {
 vec3 p=fract(vec3(mod(n,256.0),floor(n/256.0),seed)*vec3(.1031,.1030,.0973));
 p+=dot(p,p.yzx+33.33);
 return fract((p.x+p.y)*p.z);
}
// High and low words occupy separate rows, sampled at texel centers.
vec4 readWords(sampler2D image,float id,float width,bool pixels) {
 vec2 highUV=vec2((id+.5)/width,.25),lowUV=vec2((id+.5)/width,.75);
 vec4 base=pixels?vec4(1024.0,16.0,1.0,1.0):vec4(1.0);
 return IMG_NORM_PIXEL(image,highUV)*base+IMG_NORM_PIXEL(image,lowUV);
}
vec4 record(float i) { return readWords(stars,i,4097.0,false); }
vec3 randomPosition(float i,float salt) {
 return vec3(-960.0+2880.0*rnd(i*3.0+salt),-640.0+1920.0*rnd(i*3.0+1.0+salt),40.0*rnd(i*3.0+2.0+salt));
}
// Evaluate up to six bitonic stages locally (64 source reads) per host pass.
vec2 sortStage(int stage) {
 int start=0;
 for(int level=1;level<=12;level++) {
  if(stage<start+level) return vec2(exp2(float(level)),exp2(float(level-1-(stage-start))));
  start+=level;
 }
 return vec2(2.0,1.0);
}
vec4 sortRead(float id, bool pixels, bool alternate) {
 if(pixels) {
  if(alternate)return readWords(pixelScratch,id,4096.0,true);
  return readWords(projected,id,4096.0,true);
 }
 if(alternate)return readWords(scratch,id,4097.0,false);
 return record(id);
}
vec4 sortChoose(float id,vec2 kj,vec4 p,vec4 q,bool pixels) {
 bool low=mod(floor(id/kj.y),2.0)<1.0;
 bool forward=mod(floor(id/kj.x),2.0)<1.0;
 bool less=pixels ? (q.x<p.x || (q.x==p.x && q.y<p.y)) : q.z>p.z;
 bool greater=pixels ? (q.x>p.x || (q.x==p.x && q.y>p.y)) : q.z<p.z;
 return (forward==low?less:greater)?q:p;
}
vec4 sort1(float id,int stage,bool pixels,bool alternate) {
 vec2 kj=sortStage(stage);
 float partner=mod(floor(id/kj.y),2.0)<1.0?id+kj.y:id-kj.y;
 vec4 p=sortRead(id,pixels,alternate);
 vec4 q=sortRead(partner,pixels,alternate);
 return sortChoose(id,kj,p,q,pixels);
}
vec4 sort2(float id,int stage,bool pixels,bool alternate) {
 vec2 kj=sortStage(stage);
 float partner=mod(floor(id/kj.y),2.0)<1.0?id+kj.y:id-kj.y;
 vec4 p=sort1(id,stage-1,pixels,alternate);
 vec4 q=sort1(partner,stage-1,pixels,alternate);
 return sortChoose(id,kj,p,q,pixels);
}
vec4 sort3(float id,int stage,bool pixels,bool alternate) {
 vec2 kj=sortStage(stage);
 float partner=mod(floor(id/kj.y),2.0)<1.0?id+kj.y:id-kj.y;
 vec4 p=sort2(id,stage-1,pixels,alternate);
 vec4 q=sort2(partner,stage-1,pixels,alternate);
 return sortChoose(id,kj,p,q,pixels);
}
vec4 sort4(float id,int stage,bool pixels,bool alternate) {
 vec2 kj=sortStage(stage);
 float partner=mod(floor(id/kj.y),2.0)<1.0?id+kj.y:id-kj.y;
 vec4 p=sort3(id,stage-1,pixels,alternate);
 vec4 q=sort3(partner,stage-1,pixels,alternate);
 return sortChoose(id,kj,p,q,pixels);
}
vec4 sort5(float id,int stage,bool pixels,bool alternate) {
 vec2 kj=sortStage(stage);
 float partner=mod(floor(id/kj.y),2.0)<1.0?id+kj.y:id-kj.y;
 vec4 p=sort4(id,stage-1,pixels,alternate);
 vec4 q=sort4(partner,stage-1,pixels,alternate);
 return sortChoose(id,kj,p,q,pixels);
}
vec4 sort6(float id,int stage,bool pixels,bool alternate) {
 vec2 kj=sortStage(stage);
 float partner=mod(floor(id/kj.y),2.0)<1.0?id+kj.y:id-kj.y;
 vec4 p=sort5(id,stage-1,pixels,alternate);
 vec4 q=sort5(partner,stage-1,pixels,alternate);
 return sortChoose(id,kj,p,q,pixels);
}
void renderStarfield() {
 float id=floor(gl_FragCoord.x);
 if(PASSINDEX==0) {
  vec4 tag=record(4096.0);
  bool fresh=FRAMEINDEX==0 || reset || tag.x!=float(mode) || tag.y!=seed;
  if(id>=4000.0) { gl_FragColor=id==4096.0?vec4(float(mode),seed,0,0):vec4(0,0,-16384,-1); return; }
  vec4 p=record(id);
  if(fresh) p=vec4(randomPosition(id,0.0),1.0);
  float n=floor(clamp(amount,0.0,1.0)*4000.0);
  if(mode!=0 && id<n) {
   float fade=max(0.0,1.0-depth*p.z/40.0);
   // Native skips motion of black stars except in Z mode.
   float stepSize=speed*max(TIMEDELTA,0.0)*rate;
   if(fade>0.0 || mode==3) {
    if(mode==1 || mode==4 || mode==5) p.x+=stepSize;
    if(mode==2 || mode==4) p.y+=stepSize;
    if(mode==5) p.y-=stepSize;
    if(mode==3) p.z+=stepSize;
    int framePhase=FRAMEINDEX-(FRAMEINDEX/4096)*4096;
    float salt=float(framePhase)*17.0+53.0;
    if(mode==3 && (p.z<=10.0 || p.z>40.0)) {
      bool nearEnd=p.z<=10.0;
      p.xyz=randomPosition(id,salt);
      p.z=nearEnd?40.0-4.0*rnd(id+salt+9.0):10.0+4.0*rnd(id+salt+9.0);
    }
    if(mode!=3) {
      if(p.x>1920.0) { p.z=40.0*rnd(id+salt);p.x=-960.0*rnd(id+salt+1.0);p.y=-640.0+1920.0*rnd(id+salt+2.0); }
      else if(p.x< -960.0) { p.z=40.0*rnd(id+salt);p.x=960.0*(1.0+rnd(id+salt+1.0));p.y=-640.0+1920.0*rnd(id+salt+2.0); }
      if(p.y>1280.0) { p.z=40.0*rnd(id+salt);p.y=-640.0*rnd(id+salt+1.0);p.x=-960.0+2880.0*rnd(id+salt+2.0); }
      else if(p.y< -640.0) { p.z=40.0*rnd(id+salt);p.y=640.0*(1.0+rnd(id+salt+1.0));p.x=-960.0+2880.0*rnd(id+salt+2.0); }
    }
   }
   p.a=fade;
  }
  gl_FragColor=p;return;
 }
 if((PASSINDEX>=1 && PASSINDEX<=14) || (PASSINDEX>=16 && PASSINDEX<=29)) {
  bool pixels=PASSINDEX>=16;
  int group=pixels?PASSINDEX-16:PASSINDEX-1;
  bool alternate=mod(float(group),2.0)>0.5;
  if(!pixels && (!sorting || id>=4096.0)) {gl_FragColor=sortRead(id,false,alternate);return;}
  int first=group<12?group*6:72+(group-12)*3;
  if(group<12)gl_FragColor=sort6(id,first+5,pixels,alternate);
  else gl_FragColor=sort3(id,first+2,pixels,alternate);
  return;
 }
 if(PASSINDEX==15) {
  if(id>=floor(clamp(amount,0.0,1.0)*4000.0) || mode==0) {gl_FragColor=vec4(1e6,id,0,0);return;}
  vec4 p=record(id);
  int z=int(p.z)+1;
  if(z==0) {gl_FragColor=vec4(1e6,id,0,0);return;}
  // Reference pixels; Origin Y increases upward, like the arabesque controls.
  // Keep integer projection and pixel keys exact for the sorting passes.
  int cx=mode==3?int(floor(960.0*originX+0.5)):480;
  int cy=mode==3?int(floor(640.0*(1.0-originY)+0.5)):320;
  vec2 xy=vec2(float(cx)+float((10*(int(p.x)-cx))/z),float(cy)+float((10*(int(p.y)-cy))/z));
  float key=xy.y*960.0+xy.x;
  if(xy.x<=0.0 || xy.x>=960.0 || xy.y<0.0 || xy.y>=640.0)key=1e6;
  gl_FragColor=vec4(key,id,p.a,p.z);return;
 }
 vec2 pixel=floor(gl_FragCoord.xy*vec2(960.0,640.0)/RENDERSIZE+vec2(.0001));
 pixel.y=639.0-pixel.y;
 float alpha=0.0, last=-1.0;
 // Search the projected pixel index. No per-output-pixel scan of 4000 stars.
 for(int dy=-4;dy<=4;dy++) for(int dx=-4;dx<=4;dx++) {
  if(float(abs(dx))>=max(1.0,pointSize)*.5 || float(abs(dy))>=max(1.0,pointSize)*.5)continue;
  vec2 q=pixel+vec2(float(dx),float(dy));
  if(q.x<=0.0 || q.x>=960.0 || q.y<0.0 || q.y>=640.0)continue;
  float key=q.y*960.0+q.x, lo=0.0,hi=4096.0;
  for(int b=0;b<13;b++) {
   float mid=floor((lo+hi)*.5);
   if(mid>=4096.0)break;
   vec4 p=readWords(projected,mid,4096.0,true);
   if(p.x<=key)lo=mid+1.0;else hi=mid;
  }
  vec4 p=readWords(projected,max(0.0,lo-1.0),4096.0,true);
  if(p.x==key && p.y>last) {last=p.y;alpha=p.z;}
 }
 gl_FragColor=vec4(1.0,1.0,1.0,clamp(alpha*volume,0.0,1.0));
}

void main() {
 renderStarfield();
 if(PASSINDEX<30) {
  bool pixels=PASSINDEX>=15;
  vec4 base=pixels?vec4(1024.0,16.0,1.0,1.0):vec4(1.0);
  vec4 high=floor(gl_FragColor/base);
  gl_FragColor=gl_FragCoord.y<1.0?high:gl_FragColor-high*base;
 }
}
