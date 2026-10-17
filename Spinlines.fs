/*{
  "ISFVSN": "2.0",
  "VSN": "1.6-geometry.2",
  "DESCRIPTION": "Spinlines",
  "CREDIT": "codex",
  "CATEGORIES": [
    "Generator",
    "Geometry"
  ],
  "INPUTS": [
    {
      "NAME": "order",
      "LABEL": "Order",
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
      "NAME": "fill",
      "LABEL": "Fill",
      "TYPE": "bool",
      "DEFAULT": false
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
      "LABEL": "Line Width (reference pixels)",
      "TYPE": "float",
      "MIN": 0.5,
      "MAX": 4,
      "DEFAULT": 1.5
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
      "TARGET": "segments",
      "FLOAT": true,
      "WIDTH": "1024",
      "HEIGHT": "3136"
    },
    {
      "TARGET": "bounds1",
      "FLOAT": true,
      "WIDTH": "2048",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds2",
      "FLOAT": true,
      "WIDTH": "512",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds3",
      "FLOAT": true,
      "WIDTH": "128",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds4",
      "FLOAT": true,
      "WIDTH": "32",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds5",
      "FLOAT": true,
      "WIDTH": "8",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds6",
      "FLOAT": true,
      "WIDTH": "2",
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

#define TAU 6.283185307179586
#define STEP 1.2566370964050293
float countN(){return clamp(floor(order),1.0,99.0);}
float countD(){return clamp(floor(depth),1.0,98.0);}
float spacing(){return 2.0*increment*(geometryScale+.5);}
float fillCount(){return fill?floor(spacing()):1.0;}
float signedCycle(float a){return sign(a)*mod(abs(a),TAU);}
float radiusAdjust(float a){if(a>=TAU)return abs(a-TAU);if(a<0.0)return TAU-a;return a;}
float rotationAdjust(float a){if(a>=TAU)return abs(a-TAU);if(a<0.0)return TAU-abs(a);return a;}
vec2 truncPoint(vec2 a){return sign(a)*floor(abs(a));}
void clockPass(){
 vec4 old=IMG_NORM_PIXEL(phaseState,vec2(.5));
 bool reset=FRAMEINDEX==0 || resetPhases;
 if(reset)old=vec4(0.0);
 float dt=reset?0.0:max(TIMEDELTA,0.0);
 gl_FragColor=vec4(mod(old.r+rotationSpeed*dt,1.0),signedCycle(old.g+countN()*countD()*fillCount()*STEP*radiusSpeed*25.0*dt),0.0,1.0);
}
// Integer high/low words survive RGBA16F storage, including large layers.
vec4 encodeWord(vec4 value,float word){vec4 hi=floor(value/256.0);return word<.5?hi:value-hi*256.0;}
vec4 emptyBox(){return vec4(131072.0,131072.0,-131072.0,-131072.0);}
vec4 segmentForFill(float index,float layer,float fillIndex){
 float n=countN();
 if(index>=n*n || layer>=countD())return vec4(0.0);
 // Integer quotient is essential: Radeon float division can put 3/3 or 6/3
 // just below the integer, assigning the horizontal segment to the previous anchor.
 int ni=int(n),ii=int(index);
 int anchorIndex=ii/ni;
 float a=float(anchorIndex),b=float(ii-anchorIndex*ni),delta=TAU/n;
 vec4 clock=IMG_NORM_PIXEL(phaseState,vec2(.5));
 float phi=rotationAdjust(TAU*mod(clock.r+rotationPhase,1.0)+layer*rotationInc);
 float s=1.0+layer*spacing()+(fill?fillIndex+1.0:0.0);
 float angle=phi+a*delta;
 vec2 anchor=truncPoint(vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))))+s*vec2(cos(angle),sin(angle)));
 float raw=clock.g+TAU*mod(radiusPhase,1.0)+((layer*fillCount()+fillIndex)*n+a+1.0)*STEP*radiusSpeed+20.0*a*radiusInc/n;
 float modulation=radiusAmount*(1.0+.5*sin(radiusAdjust(raw)));
 float beta=phi+b*delta+modulation;
 vec2 end=truncPoint(anchor-vec2(960.0,640.0)*vec2(cos(beta),sin(beta)));
 return vec4(anchor,end);
}
vec4 segment(float index,float layer){return segmentForFill(index,layer,0.0);}
vec4 segmentAt(float i,float layer){
 float slot=2.0*i;
 vec2 uv0=vec2((mod(slot,1024.0)+.5)/1024.0,(layer*32.0+floor(slot/1024.0)+.5)/3136.0);
 vec2 uv1=vec2((mod(slot+1.0,1024.0)+.5)/1024.0,(layer*32.0+floor((slot+1.0)/1024.0)+.5)/3136.0);
 return IMG_NORM_PIXEL(segments,uv0)*256.0+IMG_NORM_PIXEL(segments,uv1);
}
vec4 boundsAt(int level,float index,float layer){
 float y=(layer+.5)/98.0;
 float w=2.0;
 if(level==1)w=2048.0;else if(level==2)w=512.0;else if(level==3)w=128.0;else if(level==4)w=32.0;else if(level==5)w=8.0;
 vec2 uv0=vec2((index*2.0+.5)/w,y),uv1=vec2((index*2.0+1.5)/w,y);
 vec4 hi,lo;
 if(level==1){hi=IMG_NORM_PIXEL(bounds1,uv0);lo=IMG_NORM_PIXEL(bounds1,uv1);}
 else if(level==2){hi=IMG_NORM_PIXEL(bounds2,uv0);lo=IMG_NORM_PIXEL(bounds2,uv1);}
 else if(level==3){hi=IMG_NORM_PIXEL(bounds3,uv0);lo=IMG_NORM_PIXEL(bounds3,uv1);}
 else if(level==4){hi=IMG_NORM_PIXEL(bounds4,uv0);lo=IMG_NORM_PIXEL(bounds4,uv1);}
 else if(level==5){hi=IMG_NORM_PIXEL(bounds5,uv0);lo=IMG_NORM_PIXEL(bounds5,uv1);}
 else {hi=IMG_NORM_PIXEL(bounds6,uv0);lo=IMG_NORM_PIXEL(bounds6,uv1);}
 return hi*256.0+lo;
}
vec4 mergeBox(vec4 a,vec4 b){return vec4(min(a.xy,b.xy),max(a.zw,b.zw));}
// Bounds enclose every sub-radius, including changing modulation directions.
vec4 familyBounds(float i,float layer,vec4 first){
 vec2 lo=min(first.xy,first.zw),hi=max(first.xy,first.zw);
 if(fill){
  vec4 last=segmentForFill(i,layer,fillCount()-1.0);
  if(radiusAmount==0.0 || radiusSpeed==0.0){
   lo=min(lo,min(last.xy,last.zw));hi=max(hi,max(last.xy,last.zw));
  }else{
   lo=min(first.xy,last.xy)-vec2(962.0,642.0);
   hi=max(first.xy,last.xy)+vec2(962.0,642.0);
  }
 }
 return vec4(lo-pointSize*.5,hi+pointSize*.5);
}
void boundsPass(){
 int level=PASSINDEX-1;
 float index=floor(gl_FragCoord.x*.5),word=mod(floor(gl_FragCoord.x),2.0),layer=floor(gl_FragCoord.y);
 vec4 box=emptyBox();
 if(layer>=countD()){gl_FragColor=encodeWord(box,word);return;}
 if(level==1){
  for(int k=0;k<16;k++){
   float i=index*16.0+float(k);
   if(i>=countN()*countN())break;
   vec4 s=segmentAt(i,layer);
   box=mergeBox(box,familyBounds(i,layer,s));
  }
 }else{
  for(int k=0;k<4;k++)box=mergeBox(box,boundsAt(level-1,index*4.0+float(k),layer));
 }
 gl_FragColor=encodeWord(box,word);
}
bool hitBox(vec2 p,vec4 b,float aa){return b.x<=b.z && all(greaterThanEqual(p,b.xy-vec2(aa))) && all(lessThanEqual(p,b.zw+vec2(aa)));}
float lineCoverage(vec2 p,vec4 s,float aa){
 vec2 v=s.zw-s.xy;
 float t=clamp(dot(p-s.xy,v)/max(dot(v,v),1.e-8),0.0,1.0);
 vec2 offset=p-s.xy-t*v;
 float d2=dot(offset,offset),limit=pointSize*.5+aa;
 if(d2>=limit*limit)return 0.0;
 return 1.0-smoothstep(-aa,aa,sqrt(d2)-pointSize*.5);
}
float cross2(vec2 a,vec2 b){return a.x*b.y-a.y*b.x;}
float fillFamilyCoverage(vec2 p,float aa,float i,float layer,vec4 first){
 float n=countN(),delta=TAU/n,f=fillCount();
 int ni=int(n),ii=int(i),a=ii/ni,b=ii-a*ni;
 vec4 clock=IMG_NORM_PIXEL(phaseState,vec2(.5));
 float phi=rotationAdjust(TAU*mod(clock.r+rotationPhase,1.0)+layer*rotationInc);
 float angle=phi+float(a)*delta;
 vec2 unit=vec2(cos(angle),sin(angle));
 float baseRadius=2.0+layer*spacing();
 float beta=phi+float(b)*delta;
 float raw=clock.g+TAU*mod(radiusPhase,1.0)+(layer*f*n+float(a)+1.0)*STEP*radiusSpeed+20.0*float(a)*radiusInc/n;
 float modulation=radiusAmount*(1.0+.5*sin(radiusAdjust(raw)));
 vec2 direction=vec2(960.0,640.0)*vec2(cos(beta+modulation),sin(beta+modulation));
 bool parallelFamily=radiusAmount==0.0 || radiusSpeed==0.0;
 int firstSub=0,lastSub=int(f)-1;
 if(parallelFamily){
  // Any contributing rasterized ray must pass within this strip around its
  // unrounded line. Truncation of both endpoints is included in the margin.
  vec2 normalDirection=direction/max(length(direction),1.e-8);
  float slope=cross2(unit,normalDirection);
  float distance0=cross2(p-(vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))))+baseRadius*unit),normalDirection);
  float margin=pointSize*.5+aa+3.0;
  if(abs(slope)>1.e-6){
   float k0=(distance0-margin)/slope,k1=(distance0+margin)/slope;
   firstSub=max(0,int(floor(min(k0,k1)))-1);
   lastSub=min(lastSub,int(ceil(max(k0,k1)))+1);
  }else if(abs(distance0)>margin)return 0.0;
 }
 float result=0.0;
 for(int visit=0;visit<882;visit++){
  int sub=firstSub+visit;
  if(sub>lastSub)break;
  vec4 ray=first;
  if(sub>0){
   vec2 anchor=truncPoint(vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))))+(1.0+layer*spacing()+(float(sub)+1.0))*unit);
   vec2 v=direction;
   if(!parallelFamily){
    float t=clock.g+TAU*mod(radiusPhase,1.0)+((layer*f+float(sub))*n+float(a)+1.0)*STEP*radiusSpeed+20.0*float(a)*radiusInc/n;
    float m=radiusAmount*(1.0+.5*sin(radiusAdjust(t)));
    v=vec2(960.0,640.0)*vec2(cos(beta+m),sin(beta+m));
   }
   ray=vec4(anchor,truncPoint(anchor-v));
  }
  result=max(result,lineCoverage(p,ray,aa));
  if(result>=.9999)return result;
 }
 return result;
}
// In Line mode the rays of each fan are evenly spaced in ellipse space.
// Only rays within this conservative angular window can cover the pixel.
// The margin includes endpoint truncation and antialiasing; no rays are dropped.
float lineFanCoverage(vec2 p,float aa){
 float result=0.0,n=countN(),delta=TAU/n;
 int ni=int(n);
 vec4 clock=IMG_NORM_PIXEL(phaseState,vec2(.5));
 float tolerance=(pointSize*.5+aa+2.0)/640.0;
 float pixelRadius=length(p-vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY)))));
 for(int layer=0;layer<98;layer++){
  if(float(layer)>=countD())break;
  float layerRadius=1.0+float(layer)*spacing();
  if(abs(pixelRadius-layerRadius)>964.0+pointSize*.5+aa)continue;
  if(!hitBox(p,boundsAt(6,0.0,float(layer)),aa))continue;
  float phi=rotationAdjust(TAU*mod(clock.r+rotationPhase,1.0)+float(layer)*rotationInc);
  for(int anchor=0;anchor<99;anchor++){
   if(anchor>=ni)break;
   int firstIndex=anchor*ni;
   vec4 firstRay=segmentAt(float(firstIndex),float(layer));
   vec2 z=(firstRay.xy-p)/vec2(960.0,640.0);
   float r=length(z);
   if(r>1.0+tolerance)continue;
   float base=phi;
   if(radiusAmount!=0.0){
    float raw=clock.g+TAU*mod(radiusPhase,1.0)+(float(layer)*n+float(anchor)+1.0)*STEP*radiusSpeed+20.0*float(anchor)*radiusInc/n;
    base+=radiusAmount*(1.0+.5*sin(radiusAdjust(raw)));
   }
   int startRay=0,rayCount=ni;
   if(r>tolerance){
    float center=(atan(z.y,z.x)-base)/delta;
    // asin(x)<=pi*x/2; use an overestimate to avoid another transcendental.
    float span=(TAU*.25)*tolerance/r/delta;
    startRay=int(floor(center-span))-1;
    rayCount=min(ni,int(ceil(center+span))+2-startRay);
   }
   for(int candidate=0;candidate<99;candidate++){
    if(candidate>=rayCount)break;
    int rawIndex=startRay+candidate;
    int b=rawIndex-(rawIndex/ni)*ni;
    if(b<0)b+=ni;
    vec4 ray=firstRay;
    if(b!=0)ray=segmentAt(float(firstIndex+b),float(layer));
    result=max(result,lineCoverage(p,ray,aa));
    if(result>=.9999)return result;
   }
  }
 }
 return result;
}
// When layer angles/directions agree, invert the radial scan instead of
// visiting every layer. Integer-spaced Fill scans concatenate into one lattice.
float radialLatticeCoverage(vec2 p,float aa){
 float result=0.0,n=countN(),delta=TAU/n,step=spacing();
 int ni=int(n),radialCount=int(countD());
 float startRadius=1.0;
 if(fill){radialCount=int(countD()*fillCount());step=1.0;startRadius=2.0;}
 vec4 clock=IMG_NORM_PIXEL(phaseState,vec2(.5));
 float phi=rotationAdjust(TAU*mod(clock.r+rotationPhase,1.0));
 float margin=pointSize*.5+aa+3.0;
 float r=length(p-vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY)))));
 int radialFirst=max(0,int(floor((r-964.0-pointSize*.5-aa-startRadius)/step))-1);
 int radialLast=min(radialCount-1,int(ceil((r+964.0+pointSize*.5+aa-startRadius)/step))+1);
 float maxCoverage=1.0-smoothstep(-aa,aa,-pointSize*.5);
 for(int anchorIndex=0;anchorIndex<99;anchorIndex++){
  if(anchorIndex>=ni)break;
  float a=float(anchorIndex),angle=phi+a*delta;
  vec2 unit=vec2(cos(angle),sin(angle));
  vec2 origin=vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))))+startRadius*unit;
  float modulation=0.0;
  if(radiusAmount!=0.0){
   float raw=clock.g+TAU*mod(radiusPhase,1.0)+(a+1.0)*STEP*radiusSpeed+20.0*a*radiusInc/n;
   modulation=radiusAmount*(1.0+.5*sin(radiusAdjust(raw)));
  }
  for(int b=0;b<99;b++){
   if(b>=ni)break;
   float beta=phi+float(b)*delta+modulation;
   vec2 v=vec2(960.0,640.0)*vec2(cos(beta),sin(beta));
   float len=length(v);vec2 vunit=v/len;
   int first=radialFirst,last=radialLast;
   float normal0=cross2(p-origin,vunit),normalStep=step*cross2(unit,vunit);
   if(abs(normalStep)>1.e-6){
    float q0=(normal0-margin)/normalStep,q1=(normal0+margin)/normalStep;
    first=max(first,int(floor(min(q0,q1)))-1);
    last=min(last,int(ceil(max(q0,q1)))+1);
   }else if(abs(normal0)>margin)continue;
   float along0=dot(p-origin,vunit),alongStep=step*dot(unit,vunit);
   if(abs(alongStep)>1.e-6){
    float q0=(along0-margin)/alongStep,q1=(along0+len+margin)/alongStep;
    first=max(first,int(floor(min(q0,q1)))-1);
    last=min(last,int(ceil(max(q0,q1)))+1);
   }else if(along0>margin || along0<-len-margin)continue;
   // Bounds above restrict the work to radial indices that can reach p.
   // The limit covers all 98*882 possible Fill scan positions.
   for(int visit=0;visit<86436;visit++){
    int q=first+visit;
    if(q>last)break;
    vec4 ray;
    if(!fill)ray=segmentAt(float(anchorIndex*ni+b),float(q));
    else{
     int scans=int(fillCount()),layer=q/scans,sub=q-layer*scans;
     float radius=1.0+float(layer)*spacing()+(float(sub)+1.0);
     vec2 base=truncPoint(vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))))+radius*unit);
     ray=vec4(base,truncPoint(base-v));
    }
    result=max(result,lineCoverage(p,ray,aa));
    if(result>=maxCoverage)return result;
   }
  }
 }
 return result;
}
float coverage(vec2 p,float aa){
 float result=0.0;
 float pixelRadius=length(p-vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY)))));
 for(int layer=0;layer<98;layer++){
  if(float(layer)>=countD())break;
  float firstRadius=1.0+float(layer)*spacing()+(fill?1.0:0.0);
  float lastRadius=firstRadius+(fill?fillCount()-1.0:0.0);
  if(pixelRadius<firstRadius-964.0-pointSize*.5-aa || pixelRadius>lastRadius+964.0+pointSize*.5+aa)continue;
  // Four-way stackless DFS: 1+4+16+64+256+1024 = 1365 nodes.
  int level=6;float index=0.0;
  for(int visit=0;visit<1365;visit++){
   bool hit=hitBox(p,boundsAt(level,index,float(layer)),aa);
   if(hit && level>1){index*=4.0;level--;continue;}
   if(hit){
    for(int k=0;k<16;k++){
     float i=index*16.0+float(k);
     if(i>=countN()*countN())break;
     vec4 s=segmentAt(i,float(layer));
     vec4 box=familyBounds(i,float(layer),s);
     if(hitBox(p,box,aa)){
      if(!fill)result=max(result,lineCoverage(p,s,aa));
      else result=max(result,fillFamilyCoverage(p,aa,i,float(layer),s));
     }
     if(result>=.9999)return result;
    }
   }
   // Skip all rejected descendants and climb past completed siblings.
   for(int climb=0;climb<5;climb++){
    if(level>=6 || mod(index,4.0)<2.5)break;
    index=floor(index/4.0);level++;
   }
   if(level>=6)break;
   index+=1.0;
  }
 }
 return result;
}
float agModeCoverage(vec2 p,float aa){

 bool sharedDirections=rotationInc==0.0 && (radiusAmount==0.0 || radiusSpeed==0.0);
 bool latticeFill=fill && spacing()==floor(spacing());
 bool useLattice=sharedDirections && (latticeFill || (!fill && countD()>=8.0 && countD()>=countN()*.5));
 if(useLattice)return radialLatticeCoverage(p,aa);
 else if(fill)return coverage(p,aa);else return lineFanCoverage(p,aa);
}
vec4 agCalculateBounds(){vec4 box=vec4(1.e20,1.e20,-1.e20,-1.e20);for(int layer=0;layer<98;layer++){if(float(layer)>=countD())break;vec4 b=boundsAt(6,0.0,float(layer));if(b.x<=b.z && b.y<=b.w)box=vec4(min(box.xy,b.xy),max(box.zw,b.zw));}return box+vec4(-pointSize-4.0,-pointSize-4.0,pointSize+4.0,pointSize+4.0);}

#define AG_SOURCE_COVERAGE agModeCoverage
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

void agOriginalMain(){
 if(PASSINDEX==0){clockPass();return;}
 if(PASSINDEX==1){
  float slot=floor(gl_FragCoord.x)+1024.0*mod(floor(gl_FragCoord.y),32.0);
  gl_FragColor=encodeWord(segment(floor(slot*.5),floor(gl_FragCoord.y/32.0)),mod(slot,2.0));return;
 }
 if(PASSINDEX<=7){boundsPass();return;}
 // Full output domain: no reference-canvas clipping or letterbox mask.
 float fit=min(RENDERSIZE.x/960.0,RENDERSIZE.y/640.0)*viewZoom;
 vec2 p=(vec2(gl_FragCoord.x,RENDERSIZE.y-gl_FragCoord.y)-RENDERSIZE*.5)/fit+vec2(480.0,320.0);
 float c=agGeometryCoverage(p,.55/fit);
 gl_FragColor=c>=1.0/255.0?vec4(vec3(1.0),c):vec4(0.0);
}

void main(){if(PASSINDEX==8){gl_FragColor=agCalculateBounds();return;}agOriginalMain();}
