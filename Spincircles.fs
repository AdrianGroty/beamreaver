/*{
  "ISFVSN": "2.0",
  "VSN": "1.1-geometry.2",
  "DESCRIPTION": "Spincircles",
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
      "LABEL": "Point Size (reference pixels)",
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
      "TARGET": "circles",
      "FLOAT": true,
      "WIDTH": "1024",
      "HEIGHT": "3136"
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
#define PI 3.141592653589793
#define DEG 0.017453292519943295
#define STEP 1.2566370964050293
float countN(){return clamp(floor(order),1.0,99.0);}
float countD(){return clamp(floor(depth),1.0,98.0);}
float spacing(){return 2.0*increment*(geometryScale+.5);}
float fillCount(){return fill?floor(spacing()):1.0;}
float signedCycle(float a){return sign(a)*mod(abs(a),TAU);}
float radiusAdjust(float a){if(a>=TAU)return abs(a-TAU);if(a<0.0)return TAU-a;return a;}
float rotationAdjust(float a){if(a>=TAU)return abs(a-TAU);if(a<0.0)return TAU-abs(a);return a;}
float truncZero(float a){return sign(a)*floor(abs(a));}
vec2 truncPoint(vec2 a){return sign(a)*floor(abs(a));}
void clockPass(){
 vec2 uv=vec2(.5);vec4 old=IMG_NORM_PIXEL(phaseState,uv);
 bool reset=FRAMEINDEX==0 || resetPhases;
 if(reset)old=vec4(0.0);
 float dt=reset?0.0:max(TIMEDELTA,0.0);
 gl_FragColor=vec4(mod(old.r+rotationSpeed*dt,1.0),signedCycle(old.g+countN()*countD()*fillCount()*STEP*radiusSpeed*25.0*dt),0.0,1.0);
}
float angleStep(){
 int n=int(countN());
 if(n==1)return 6.2831854820251465;
 if(n==2)return 3.1415927410125732;
 if(n==3)return 2.094395160675049;
 if(n==4)return 1.5707963705062866;
 if(n==5)return 1.2566370964050293;
 if(n==6)return 1.0471975803375244;
 if(n==7)return 0.8975979089736938;
 if(n==8)return 0.7853981852531433;
 if(n==9)return 0.6981316804885864;
 if(n==10)return 0.6283185482025146;
 return TAU/countN();
}
// Replay the native float cursor for its supported Order range. Extended
// Orders use stable periodic indexed angles to avoid O(N^4) cache generation
// and large-angle precision loss; the native renderer caps Order at 10.
float cursorAngle(float phi,int index){
 float delta=angleStep();
 if(countN()<=10.0){
  float a=phi;
  for(int k=0;k<100;k++){if(k>=index)break;a+=delta;}
  return a;
 }
 int ni=int(countN()),wrapped=index-(index/ni)*ni;
 return phi+float(wrapped)*delta;
}
vec4 currentClock(){vec2 uv=vec2(.5);return IMG_NORM_PIXEL(phaseState,uv);}
float layerPhi(float layer,vec4 clock){return rotationAdjust(TAU*mod(clock.r+rotationPhase,1.0)+layer*rotationInc);}
float layerRadius(float layer,float sub){return 1.0+layer*spacing()+(fill?sub+1.0:0.0);}
float modGain(float layer,float sub,float a,vec4 clock){
 float n=countN();
 float raw=clock.g+TAU*mod(radiusPhase,1.0)+((layer*fillCount()+sub)*n+a+1.0)*STEP*radiusSpeed+20.0*a*radiusInc/n;
 return radiusAmount*(1.0+.5*sin(radiusAdjust(raw)));
}
vec4 circleDescriptor(float i,float layer,float sub){
 int n=int(countN()),ii=int(i),a=ii/n;
 if(ii>=n*n || layer>=countD())return vec4(0.0);
 vec4 clock=currentClock();float phi=layerPhi(layer,clock),s=layerRadius(layer,sub);
 float angle=cursorAngle(phi,a);
 vec2 c=truncPoint(vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))))+s*vec2(cos(angle),sin(angle)));
 float gain=modGain(layer,sub,float(a),clock);
 float radius=s*gain+s;
 return vec4(c,truncZero(radius*cos(cursorAngle(phi,ii))),1.0);
}
// Both words contain exactly representable integers even in RGBA16F.
vec4 encodeWord(vec4 v,float word){vec4 hi=floor(v/256.0);return word<.5?hi:v-hi*256.0;}
vec4 circleAt(float i,float layer){
 float slot=2.0*i;
 vec2 uv0=vec2((mod(slot,1024.0)+.5)/1024.0,(layer*32.0+floor(slot/1024.0)+.5)/3136.0);
 vec2 uv1=vec2((mod(slot+1.0,1024.0)+.5)/1024.0,(layer*32.0+floor((slot+1.0)/1024.0)+.5)/3136.0);
 return IMG_NORM_PIXEL(circles,uv0)*256.0+IMG_NORM_PIXEL(circles,uv1);
}
float pointCoverage(vec2 p,vec2 anchor,float aa){
 vec2 q=abs(p-anchor-vec2(pointSize*.5))-vec2(pointSize*.5);
 float d=length(max(q,vec2(0.0)))+min(max(q.x,q.y),0.0);
 return 1.0-smoothstep(-aa,aa,d);
}
int wrappedIndex(int value,int n){int b=value-(value/n)*n;return b<0?b+n:b;}
// Select all potentially contributing native degree samples. Quantization,
// square point footprint and AA are included in the conservative window.
float sampledCircle(vec2 p,vec4 circle,float aa){
 if(circle.w<.5)return 0.0;
 float radius=abs(circle.z);
 if(radius<.5)return pointCoverage(p,circle.xy,aa);
 vec2 z=p-vec2(pointSize*.5)-circle.xy;
 float d=length(z),margin=pointSize*.707106782+aa+1.5;
 if(abs(d-radius)>margin)return 0.0;
 int first=0,count=360;
 if(d>margin){
  float center=(atan(z.y,z.x)-(circle.z<0.0?PI:0.0))/DEG;
  // Tangent angle of the conservative footprint disk; no extra degree
  // samples outside this interval can contribute.
  float span=asin(clamp(margin/d,0.0,1.0))/DEG;
  first=int(ceil(center-span-.001));
  count=min(360,max(0,int(floor(center+span+.001))+1-first));
 }
 float result=0.0,maxCoverage=1.0-smoothstep(-aa,aa,-pointSize*.5);
 for(int k=0;k<360;k++){
  if(k>=count)break;
  int t=wrappedIndex(first+k,360);float angle=float(t)*DEG;
  vec2 point=truncPoint(circle.xy+circle.z*vec2(cos(angle),sin(angle)));
  result=max(result,pointCoverage(p,point,aa));
  if(result>=maxCoverage)return result;
 }
 return result;
}
float selectedCircle(vec2 p,float aa,float layer,int a,int b){
 return sampledCircle(p,circleAt(float(a*int(countN())+b),layer),aa);
}
float lineCoverage(vec2 p,float aa){
 float result=0.0,n=countN(),delta=TAU/n;
 float maxCoverage=1.0-smoothstep(-aa,aa,-pointSize*.5);
 float margin=pointSize*.707106782+aa+2.5;
 int ni=int(n);vec4 clock=currentClock();
 for(int layer=0;layer<98;layer++){
  if(float(layer)>=countD())break;
  float phi=layerPhi(float(layer),clock),s=layerRadius(float(layer),0.0);
  // Every center lies at distance S; no circle exceeds S*(1+1.5*A).
  if(length(p-vec2(pointSize*.5)-vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY)))))>s*(2.0+1.5*radiusAmount)+margin+2.0)continue;
  for(int a=0;a<99;a++){
   if(a>=ni)break;
   vec4 first=circleAt(float(a*ni),float(layer));
   float distanceToCenter=length(p-vec2(pointSize*.5)-first.xy);
   float gain=modGain(float(layer),0.0,float(a),clock),r=s*gain+s;
   if(distanceToCenter>r+margin)continue;
   // Four cosine preimages select circle radii close to this pixel's radius.
   // A guard covers the native float angular cursor's accumulated error.
   bool allRadii=ni<=24 || r<=margin;
   float lo=0.0,hi=PI*.5;
   if(!allRadii){
    lo=acos(clamp((distanceToCenter+margin)/r,0.0,1.0));
    hi=acos(clamp((distanceToCenter-margin)/r,0.0,1.0));
    if(4.0*(ceil((hi-lo+.0005)/delta)+1.0)>=n)allRadii=true;
   }
   if(allRadii){
    for(int b=0;b<99;b++){
     if(b>=ni)break;
     result=max(result,selectedCircle(p,aa,float(layer),a,b));
     if(result>=maxCoverage)return result;
    }
   }else{
    for(int branch=0;branch<4;branch++){
     float low=lo,high=hi;
     if(branch==1){low=-hi;high=-lo;}
     else if(branch==2){low=PI-hi;high=PI-lo;}
     else if(branch==3){low=PI+lo;high=PI+hi;}
     int start=int(ceil((low-phi-.00025)/delta));
     int count=min(ni,max(0,int(floor((high-phi+.00025)/delta))+1-start));
     for(int visit=0;visit<99;visit++){
      if(visit>=count)break;
      int b=wrappedIndex(start+visit,ni);
      result=max(result,selectedCircle(p,aa,float(layer),a,b));
      if(result>=maxCoverage)return result;
     }
    }
   }
  }
 }
 return result;
}
// A circle family moves along a straight radial segment as Fill advances.
// This conservative interval test prunes whole blocks of sub-radii.
bool radialFamilyMayHit(vec2 p,vec2 unit,float c,float s0,float s1,float aa,float gainLow,float gainHigh){
 vec2 z=p-vec2(pointSize*.5)-vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))));
 float unitLength=length(unit);vec2 u=unit/unitLength;
 float parallel=dot(z,u),perp=abs(z.x*u.y-z.y*u.x);
 float t0=s0*unitLength,t1=s1*unitLength;
 float k0=abs(c)*gainLow/unitLength,k1=abs(c)*gainHigh/unitLength;
 // g(t)=sqrt((t-parallel)^2+perp^2)-k*t is convex. Its exact
 // interval minimum/maximum reject circle families without losing tangencies.
 float nearest=t1;
 if(k1<1.0)nearest=clamp(parallel+k1*perp/sqrt(max(1.0-k1*k1,1.e-20)),t0,t1);
 float lo=length(vec2(nearest-parallel,perp))-k1*nearest;
 float hi=max(length(vec2(t0-parallel,perp))-k0*t0,length(vec2(t1-parallel,perp))-k0*t1);
 float margin=pointSize*.707106782+aa+4.0;
 return lo<=margin && hi>=-margin;
}
float fillCoverage(vec2 p,float aa){
 float result=0.0,n=countN(),f=fillCount();int ni=int(n);
 float maxCoverage=1.0-smoothstep(-aa,aa,-pointSize*.5);
 float leaves=exp2(ceil(log2(max(f,1.0))));
 vec4 clock=currentClock();
 for(int layer=0;layer<98;layer++){
  if(float(layer)>=countD())break;
  float phi=layerPhi(float(layer),clock);
  float sMax=layerRadius(float(layer),f-1.0);
  vec2 relative=p-vec2(pointSize*.5)-vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))));
  float pad=pointSize*.707106782+aa+4.0;
  if(length(relative)>sMax*(2.0+1.5*radiusAmount)+pad)continue;
  for(int a=0;a<99;a++){
   if(a>=ni)break;
   float angle=cursorAngle(phi,a);vec2 unit=vec2(cos(angle),sin(angle));
   float g0=1.0+.5*radiusAmount,g1=1.0+1.5*radiusAmount;
   if(radiusAmount==0.0 || radiusSpeed==0.0){g0=1.0+modGain(float(layer),0.0,float(a),clock);g1=g0;}
   if(any(greaterThan(abs(relative-sMax*unit),vec2(sMax*g1+pad))))continue;
   for(int b=0;b<99;b++){
    if(b>=ni)break;
    float cosine=cos(cursorAngle(phi,a*ni+b));
    vec2 end0=layerRadius(float(layer),0.0)*unit,end1=sMax*unit;
    float maxRadius=abs(cosine)*sMax*g1+pad;
    if(any(lessThan(relative,min(end0,end1)-vec2(maxRadius))) || any(greaterThan(relative,max(end0,end1)+vec2(maxRadius))))continue;
    float node=1.0,width=1.0,span=leaves;
    for(int visit=0;visit<2047;visit++){
     float low=(node-width)*span,high=min(low+span-1.0,f-1.0);
     bool hit=low<=high && radialFamilyMayHit(p,unit,cosine,layerRadius(float(layer),low),layerRadius(float(layer),high),aa,g0,g1);
     if(hit && span>1.0){node*=2.0;width*=2.0;span*=.5;continue;}
     if(hit){
      float s=layerRadius(float(layer),low);
      float gain=modGain(float(layer),low,float(a),clock);
      vec2 center=truncPoint(vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))))+s*unit);
      float radius=truncZero((s*gain+s)*cosine);
      result=max(result,sampledCircle(p,vec4(center,radius,1.0),aa));
      if(result>=maxCoverage)return result;
     }
     for(int climb=0;climb<10;climb++){
      if(node<=1.0 || mod(node,2.0)<.5)break;
      node=floor(node*.5);width*=.5;span*=2.0;
     }
     if(node<=1.0)break;
     node+=1.0;
    }
   }
  }
 }
 return result;
}
float agModeCoverage(vec2 p,float aa){if(fill)return fillCoverage(p,aa);return lineCoverage(p,aa);}
vec4 agCalculateBounds(){vec4 box=vec4(1.e20,1.e20,-1.e20,-1.e20);float r=layerRadius(countD()-1.0,fillCount()-1.0)*(2.0+1.5*radiusAmount);vec2 o=vec2(float(int(960.0*originX)),float(int(640.0*(1.0-originY))));box=vec4(o-vec2(r),o+vec2(r));return box+vec4(-pointSize-4.0,-pointSize-4.0,pointSize+4.0,pointSize+4.0);}

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
  gl_FragColor=encodeWord(circleDescriptor(floor(slot*.5),floor(gl_FragCoord.y/32.0),0.0),mod(slot,2.0));return;
 }
 float fit=min(RENDERSIZE.x/960.0,RENDERSIZE.y/640.0)*viewZoom;
 vec2 p=(vec2(gl_FragCoord.x,RENDERSIZE.y-gl_FragCoord.y)-RENDERSIZE*.5)/fit+vec2(480.0,320.0);
 float c=agGeometryCoverage(p,.55/fit);
 gl_FragColor=c>=1.0/255.0?vec4(vec3(1.0),c):vec4(0.0);
}

void main(){if(PASSINDEX==2){gl_FragColor=agCalculateBounds();return;}agOriginalMain();}
