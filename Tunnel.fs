/*{
  "ISFVSN": "2.0",
  "VSN": "1.1-geometry.2",
  "DESCRIPTION": "Tunnel",
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
      "DEFAULT": 24
    },
    {
      "NAME": "increment",
      "LABEL": "Recursion Increment",
      "TYPE": "float",
      "MIN": 1,
      "MAX": 49,
      "DEFAULT": 40
    },
    {
      "NAME": "fill",
      "LABEL": "Fill",
      "TYPE": "bool",
      "DEFAULT": false
    },
    {
      "NAME": "rotationSpeed",
      "LABEL": "Rotation Speed (cycles/sec)",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0.03
    },
    {
      "NAME": "rotationInc",
      "LABEL": "Rotation Inc (native: inactive in Tunnel)",
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
      "DEFAULT": 0
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
      "MIN": 0.01,
      "MAX": 0.5,
      "DEFAULT": 0.25
    },
    {
      "NAME": "originX",
      "LABEL": "Contraction Origin X",
      "TYPE": "float",
      "MIN": 0,
      "MAX": 1,
      "DEFAULT": 0.5
    },
    {
      "NAME": "originY",
      "LABEL": "Contraction Origin Y",
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
      "DEFAULT": 1
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
      "WIDTH": "2",
      "HEIGHT": "1"
    },
    {
      "TARGET": "points",
      "FLOAT": true,
      "WIDTH": "1024",
      "HEIGHT": "784"
    },
    {
      "TARGET": "bounds1",
      "FLOAT": true,
      "WIDTH": "1024",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds2",
      "FLOAT": true,
      "WIDTH": "256",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds3",
      "FLOAT": true,
      "WIDTH": "64",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds4",
      "FLOAT": true,
      "WIDTH": "16",
      "HEIGHT": "98"
    },
    {
      "TARGET": "bounds5",
      "FLOAT": true,
      "WIDTH": "4",
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
#define TAU 6.283185307179586
#define HALFPI 1.570796326794897
#define RSTEP 0.157079637050629
float countN(){return clamp(floor(order),1.0,99.0);}
float countD(){return clamp(floor(depth),1.0,98.0);}
int perSide(){return 3200/int(countN());}
int sampleCount(){return int(countN())*perSide();}
// Published control ranges guarantee 1.02 <= B <= 98, below the native pole.
float spacing(){return 2.0*clamp(increment,1.0,49.0)*(clamp(geometryScale,.01,.5)+.5);}
float fillCount(){return fill?floor(spacing()):0.0;}
vec2 truncPoint(vec2 p){return sign(p)*floor(abs(p));}
// Constant-step native radius clock. Negative steps reflect at zero instead
// of ordinary wrapping. After the first reflection, two alternating phases
// recur after equal integer sample counts, allowing a constant-time prefix.
float advanceRadius(float p,float stepSize,float samples){
 if(samples<=0.0 || stepSize==0.0)return p;
 if(stepSize>0.0)return mod(p+samples*stepSize,TAU);
 float a=-stepSize,first=floor(p/a)+1.0;
 if(samples<first)return p-samples*a;
 float high=TAU+first*a-p;
 // At the exact endpoint the next subtraction lands on TAU, whose native
 // >= branch wraps to zero. This yields the special zero/high two-cycle.
 if(high-a>=TAU)return mod(samples-first,2.0)<.5?high:0.0;
 float period=floor(high/a)+1.0;
 float left=samples-first,blocks=floor(left/period);
 float start=mod(blocks,2.0)<.5?high:TAU+period*a-high;
 return start-(left-blocks*period)*a;
}
vec4 clockValue(){return IMG_NORM_PIXEL(phaseState,vec2(.25,.5))+IMG_NORM_PIXEL(phaseState,vec2(.75,.5));}
vec2 vertexUnit(float angle){
 vec2 v=vec2(cos(angle),sin(angle));
 if(abs(v.x)<1.e-6)v.x=0.0;
 if(abs(v.y)<1.e-6)v.y=0.0;
 return v;
}
void clockPass(){
 vec4 old=clockValue();bool reset=FRAMEINDEX==0 || resetPhases;
 if(reset)old=vec4(0.0);
 float dt=reset?0.0:max(TIMEDELTA,0.0);
 float total=old.b+float(sampleCount())*countD()*25.0*dt;
 float steps=floor(total);
 vec4 value=vec4(mod(old.r+rotationSpeed*dt,1.0),advanceRadius(old.g,RSTEP*radiusSpeed,steps),total-steps,0.0);
 vec4 hi=floor(value*256.0)/256.0;
 gl_FragColor=gl_FragCoord.x<1.0?hi:value-hi;
}
// Integer quotient/remainder avoids floating division selecting the previous
// polygon side on Radeon. Native traversal is sampled, not a smooth line.
vec2 initialPoint(int i,vec4 clock){
 int k=perSide(),side=i/k,t=i-side*k;
 float phi=TAU*mod(clock.r+rotationPhase,1.0)-HALFPI;
 float a=phi+float(side)*TAU/countN(),b=phi+float(side+1)*TAU/countN();
 float radius=1280.0*clamp(geometryScale,.01,.5);
 vec2 v=vertexUnit(a),w=vertexUnit(b);
 return vec2(480.0,320.0)+radius*(v+(w-v)*(float(t)/float(k)));
}
vec4 descriptor(int i,int layer){
 if(i>=sampleCount() || float(layer)>=countD())return vec4(0.0);
 vec4 clock=clockValue();float b=spacing();
 // Scalar inputs stay normalized in Hexcomposer; point2D uniforms are
 // multiplied by the current pass dimensions before being uploaded.
 vec2 target=vec2(960.0*originX,640.0*(1.0-originY));
 vec2 p=initialPoint(i,clock);
 for(int j=0;j<98;j++){if(j>=layer)break;p+=(target-p)/(100.0-b);}
 vec2 delta=target-p;
 float serial=float(layer*sampleCount()+i+1);
 float phase=advanceRadius(clock.g,RSTEP*radiusSpeed,serial);
 float h=clock.g+float(layer+1)*max(20.0*radiusInc,0.0);
 float wave=radiusAmount*b*sin(phase+h+TAU*mod(radiusPhase,1.0));
 vec2 start=truncPoint(p)+sign(delta)*wave;
 vec2 stepSize=delta/(b*(100.0-b));
 return vec4(start,stepSize);
}
vec4 encodeDescriptor(vec4 value,float word){
 vec4 hi=floor(value);
 return word<.5?hi:floor((value-hi)*2048.0+.5)/2048.0;
}
vec4 pointAt(float i,float layer){
 float slot=2.0*i;
 vec2 uv=vec2((mod(slot,1024.0)+.5)/1024.0,(layer*8.0+floor(slot/1024.0)+.5)/784.0);
 vec4 hi=floor(IMG_NORM_PIXEL(points,uv)+.5);
 vec4 lo=floor(IMG_NORM_PIXEL(points,uv+vec2(1.0/1024.0,0.0))*2048.0+.5)/2048.0;
 return hi+lo;
}
vec4 emptyBox(){return vec4(2048.0,2048.0,-2048.0,-2048.0);}
vec4 mergeBox(vec4 a,vec4 b){return vec4(min(a.xy,b.xy),max(a.zw,b.zw));}
vec4 familyBox(vec4 point){
 vec2 end=point.xy+max(fillCount()-1.0,0.0)*point.zw;
 // Include truncation, half-float residual error, footprint and integer bounds.
 return vec4(floor(min(point.xy,end))-2.0,ceil(max(point.xy,end))+pointSize+2.0);
}
float boundsWidth(int level){
 if(level==1)return 1024.0;if(level==2)return 256.0;if(level==3)return 64.0;
 if(level==4)return 16.0;if(level==5)return 4.0;return 1.0;
}
vec4 boxAt(int level,float index,float layer){
 vec2 uv=vec2((index+.5)/boundsWidth(level),(layer+.5)/98.0);
 if(level==1)return IMG_NORM_PIXEL(bounds1,uv);
 if(level==2)return IMG_NORM_PIXEL(bounds2,uv);
 if(level==3)return IMG_NORM_PIXEL(bounds3,uv);
 if(level==4)return IMG_NORM_PIXEL(bounds4,uv);
 if(level==5)return IMG_NORM_PIXEL(bounds5,uv);
 return IMG_NORM_PIXEL(bounds6,uv);
}
void boundsPass(){
 float index=floor(gl_FragCoord.x),layer=floor(gl_FragCoord.y);int level=PASSINDEX-1;
 vec4 box=emptyBox();
 if(layer<countD()){
  for(int k=0;k<4;k++){
   float i=index*4.0+float(k);
   if(level==1){if(i<float(sampleCount()))box=mergeBox(box,familyBox(pointAt(i,layer)));}
   else box=mergeBox(box,boxAt(level-1,i,layer));
  }
 }
 gl_FragColor=box;
}
bool hitBox(vec2 p,vec4 box,float aa){return all(greaterThanEqual(p,box.xy-vec2(aa))) && all(lessThanEqual(p,box.zw+vec2(aa)));}
float pointCoverage(vec2 p,vec2 anchor,float aa){
 vec2 q=abs(p-anchor-vec2(pointSize*.5))-vec2(pointSize*.5);
 float d=length(max(q,vec2(0.0)))+min(max(q.x,q.y),0.0);
 return 1.0-smoothstep(-aa,aa,d);
}
// All plotted anchors are integers. The nearest integer square gives a
// tighter attainable upper bound than the continuous square's peak alpha.
float maximumCoverage(vec2 p,float aa){
 return pointCoverage(p,floor(p-vec2(pointSize*.5)+vec2(.5)),aa);
}
float familyCoverage(vec2 p,vec4 family,float aa){
 float result=pointCoverage(p,truncPoint(family.xy),aa);
 float maximum=maximumCoverage(p,aa);
 if(!fill || result>=maximum)return result;
 // Select every integer interpolation index that could cover this pixel.
 // The extra margin contains truncation and the square point's footprint.
 float low=0.0,high=fillCount()-1.0;
 vec2 center=p-vec2(pointSize*.5),margin=vec2(pointSize*.5+aa+1.01);
 for(int axis=0;axis<2;axis++){
  float v=axis==0?family.z:family.w;
  float z=axis==0?center.x-family.x:center.y-family.y;
  float m=axis==0?margin.x:margin.y;
  if(abs(v)<1.e-10){if(abs(z)>m)return result;}
  else{float a=(z-m)/v,b=(z+m)/v;low=max(low,min(a,b));high=min(high,max(a,b));}
 }
 // t=0 duplicates the base point, which has already been evaluated.
 int first=max(1,int(ceil(low))),last=min(int(fillCount())-1,int(floor(high)));
 for(int visit=0;visit<98;visit++){
  int t=first+visit;if(t>last)break;
  result=max(result,pointCoverage(p,truncPoint(family.xy+float(t)*family.zw),aa));
  if(result>=maximum)return result;
 }
 return result;
}
float coverage(vec2 p,float aa){
 float result=0.0,maximum=maximumCoverage(p,aa);
 for(int layer=0;layer<98;layer++){
  if(float(layer)>=countD())break;
  int level=6;float index=0.0;
  for(int visit=0;visit<1365;visit++){
   bool hit=hitBox(p,boxAt(level,index,float(layer)),aa);
   if(hit && level>1){index*=4.0;level--;continue;}
   if(hit){
    for(int k=0;k<4;k++){
     float i=index*4.0+float(k);if(i>=float(sampleCount()))break;
     vec4 family=pointAt(i,float(layer));
     if(hitBox(p,familyBox(family),aa))result=max(result,familyCoverage(p,family,aa));
     if(result>=maximum)return result;
    }
   }
   for(int climb=0;climb<5;climb++){
    if(level>=6 || mod(index,4.0)<2.5)break;
    index=floor(index/4.0);level++;
   }
   if(level>=6)break;index+=1.0;
  }
 }
 return result;
}
vec4 agCalculateBounds(){vec4 box=vec4(1.e20,1.e20,-1.e20,-1.e20);for(int layer=0;layer<98;layer++){if(float(layer)>=countD())break;vec4 b=boxAt(6,0.0,float(layer));if(b.x<=b.z && b.y<=b.w)box=vec4(min(box.xy,b.xy),max(box.zw,b.zw));}return box+vec4(-pointSize-4.0,-pointSize-4.0,pointSize+4.0,pointSize+4.0);}

#define AG_SOURCE_COVERAGE coverage
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
  float slot=floor(gl_FragCoord.x)+1024.0*mod(floor(gl_FragCoord.y),8.0);
  gl_FragColor=encodeDescriptor(descriptor(int(floor(slot*.5)),int(floor(gl_FragCoord.y/8.0))),mod(slot,2.0));return;
 }
 if(PASSINDEX<=7){boundsPass();return;}
 float fit=min(RENDERSIZE.x/960.0,RENDERSIZE.y/640.0)*viewZoom;
 vec2 p=(vec2(gl_FragCoord.x,RENDERSIZE.y-gl_FragCoord.y)-RENDERSIZE*.5)/fit+vec2(480.0,320.0);
 float c=agGeometryCoverage(p,.55/fit);
 gl_FragColor=c>=1.0/255.0?vec4(vec3(1.0),c):vec4(0.0);
}

void main(){if(PASSINDEX==8){gl_FragColor=agCalculateBounds();return;}agOriginalMain();}
