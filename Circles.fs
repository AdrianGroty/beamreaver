/*{
  "ISFVSN": "2.0",
  "VSN": "1.0-geometry.2",
  "DESCRIPTION": "Circles",
  "CREDIT": "codex",
  "CATEGORIES": [
    "Generator",
    "Geometry"
  ],
  "INPUTS": [
    {
      "NAME": "order",
      "LABEL": "Order (native: inactive in Circles)",
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
      "LABEL": "Rotation Speed (native: inactive in Circles)",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "rotationInc",
      "LABEL": "Rotation Inc (native: inactive in Circles)",
      "TYPE": "float",
      "MIN": -1,
      "MAX": 1,
      "DEFAULT": 0
    },
    {
      "NAME": "rotationPhase",
      "LABEL": "Rotation Phase (native: inactive in Circles)",
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
      "WIDTH": "3",
      "HEIGHT": "1"
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
#define DEG 0.017453292519943295
float countD(){return clamp(floor(depth),1.0,98.0);}
float spacing(){return 2.0*clamp(increment,1.0,98.0)*(clamp(geometryScale,0.0,4.0)+.5);}
int scans(){return fill?int(floor(spacing())):1;}
vec2 originPoint(){return vec2(960.0*originX,640.0*(1.0-originY));}
vec2 truncPoint(vec2 p){return sign(p)*floor(abs(p));}
// Byte-exact persistent float32 values; no floating render targets needed.
vec4 packFloat(float value){
 float a=abs(value);if(!(a>0.0))return vec4(0.0);
 float e=floor(log2(a)),power=exp2(e);
 if(a<power){e-=1.0;power*=.5;}
 if(a>=power*2.0){e+=1.0;power*=2.0;}
 float m=floor((a/power-1.0)*8388608.0+.5);
 if(m>=8388608.0){m=0.0;e+=1.0;}
 float exponent=e+127.0;
 return vec4((value<0.0?128.0:0.0)+floor(exponent*.5),mod(exponent,2.0)*128.0+floor(m/65536.0),mod(floor(m/256.0),256.0),mod(m,256.0))/255.0;
}
float unpackFloat(vec4 pixel){
 vec4 b=floor(pixel*255.0+.5);if(dot(b,vec4(1.0))==0.0)return 0.0;
 float e=mod(b.r,128.0)*2.0+floor(b.g/128.0)-127.0;
 float m=mod(b.g,128.0)*65536.0+b.b*256.0+b.a;
 return (b.r>=128.0?-1.0:1.0)*(1.0+m/8388608.0)*exp2(e);
}
vec3 clockValue(){
 return vec3(unpackFloat(IMG_NORM_PIXEL(phaseState,vec2(1.0/6.0,.5))),
             unpackFloat(IMG_NORM_PIXEL(phaseState,vec2(.5,.5))),
             unpackFloat(IMG_NORM_PIXEL(phaseState,vec2(5.0/6.0,.5))));
}
void clockPass(){
 vec3 old=clockValue();bool reset=FRAMEINDEX==0 || resetPhases;
 if(reset)old=vec3(0.0);
 float dt=reset?0.0:max(TIMEDELTA,0.0);
 // r: common rotation cycles (intentionally unused by Circles geometry).
 // g: whole signed radius cycles; b: fractional cycles, kept separately so
 // the oscillator does not lose fractional precision as its clock grows.
 float next=old.b+90.0*countD()*float(scans())*radiusSpeed*25.0*dt;
 vec3 value=vec3(mod(old.r+rotationSpeed*dt,1.0),old.g+floor(next),mod(next,1.0));
 float x=floor(gl_FragCoord.x);
 gl_FragColor=packFloat(x<.5?value.r:(x<1.5?value.g:value.b));
}
float baseRadius(int layer,int sub){
 float s=1.0+float(layer)*spacing();
 return floor(fill?s+1.0: s)+float(sub);
}
float radialWave(int t,int layer,int sub,vec3 clock){
 // Signed whole cycles matter: the native negative-argument branch reflects
 // sine rather than playing an ordinary signed wrapped phase backward.
 int invocation=layer*scans()+sub;
 float ringCycles=float(invocation)*(90.0*radiusSpeed);
 float spatial=float(t)*.0027777778450399637*radiusInc/TAU;
 float cycles=clock.b+ringCycles+float(t+1)*(.25*radiusSpeed)+spatial+mod(radiusPhase,1.0);
 float wave=sin(TAU*mod(cycles,1.0));
 return clock.g+cycles<0.0?-wave:wave;
}
vec2 generatedPoint(int t,int layer,int sub,vec3 clock){
 float b=spacing(),radius=baseRadius(layer,sub);
 if(radiusAmount!=0.0)radius+=b*radiusAmount*(1.0+.5*radialWave(t,layer,sub,clock));
 float angle=float(t)*DEG;
 return truncPoint(originPoint()+radius*vec2(cos(angle),sin(angle)));
}
float pointCoverage(vec2 p,vec2 anchor,float aa){
 vec2 q=abs(p-anchor-vec2(pointSize*.5))-vec2(pointSize*.5);
 float d=length(max(q,vec2(0.0)))+min(max(q.x,q.y),0.0);
 return 1.0-smoothstep(-aa,aa,d);
}
int wrappedDegree(int t){int result=t-(t/360)*360;return result<0?result+360:result;}
float coverage(vec2 p,float aa){
 vec2 z=p-vec2(pointSize*.5)-originPoint();float rho=length(z);
 // Circle around the footprint, including final coordinate truncation.
 float margin=pointSize*.707106782+aa+1.51;
 float modulation=spacing()*radiusAmount,lowMod=.5*modulation,highMod=1.5*modulation;
 int firstDegree=0,degreeCount=360;
 if(rho>margin){
  float center=atan(z.y,z.x)/DEG,span=asin(clamp(margin/rho,0.0,1.0))/DEG;
  firstDegree=int(ceil(center-span-.001));
  degreeCount=min(360,max(0,int(floor(center+span+.001))+1-firstDegree));
 }
 float result=0.0;
 float maximum=pointCoverage(p,floor(p-vec2(pointSize*.5)+vec2(.5)),aa);
 float b=spacing(),f=float(scans());
 // Conservative layer windows account for base truncation and all possible
 // modulation values. No angle or invocation samples are reduced.
 float startOffset=fill?1.0:0.0,endOffset=fill?f+1.0:1.0;
 int firstLayer=max(0,int(ceil((rho-margin-endOffset-highMod)/b))-1);
 int lastLayer=min(int(countD())-1,int(floor((rho+margin-startOffset-lowMod)/b))+1);
 vec3 clock=clockValue();
 for(int visit=0;visit<360;visit++){
  if(visit>=degreeCount)break;int t=wrappedDegree(firstDegree+visit);
  for(int k=0;k<98;k++){
   int layer=firstLayer+k;if(layer>lastLayer)break;
   int firstSub=0,lastSub=0;
   if(fill){
    float base=baseRadius(layer,0);
    firstSub=max(0,int(ceil(rho-margin-base-highMod))-1);
    lastSub=min(scans()-1,int(floor(rho+margin-base-lowMod))+1);
   }
   for(int h=0;h<882;h++){
    int sub=firstSub+h;if(sub>lastSub)break;
    result=max(result,pointCoverage(p,generatedPoint(t,layer,sub,clock),aa));
    if(result>=maximum)return result;
   }
  }
 }
 return result;
}
vec4 agCalculateBounds(){vec4 box=vec4(1.e20,1.e20,-1.e20,-1.e20);float r=baseRadius(int(countD())-1,scans()-1)+1.5*spacing()*radiusAmount;vec2 o=originPoint();box=vec4(o-vec2(r),o+vec2(r));return box+vec4(-pointSize-4.0,-pointSize-4.0,pointSize+4.0,pointSize+4.0);}

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
 float fit=min(RENDERSIZE.x/960.0,RENDERSIZE.y/640.0)*viewZoom;
 vec2 p=(vec2(gl_FragCoord.x,RENDERSIZE.y-gl_FragCoord.y)-RENDERSIZE*.5)/fit+vec2(480.0,320.0);
 float c=agGeometryCoverage(p,.55/fit);
 gl_FragColor=c>=1.0/255.0?vec4(vec3(1.0),c):vec4(0.0);
}

void main(){if(PASSINDEX==1){gl_FragColor=agCalculateBounds();return;}agOriginalMain();}
