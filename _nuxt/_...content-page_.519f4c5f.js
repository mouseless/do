import D from"./ContentRenderer.f0f7145b.js";import{_ as T}from"./nuxt-link.0ac70c5f.js";import{a as O,m as M,u as d,o as h,b as v,W as y,w as x,k as l,X as $,Y as w,e as k,Z as L,$ as W,f as I,C as S,D as R,E as z,a0 as N,a1 as B,j as P,a2 as E,K as F,c as q}from"./entry.42953906.js";import{u as V}from"./pageStore.7288330c.js";import K from"./ContentDoc.caac30c0.js";import{q as A}from"./query.060b90cc.js";import"./ContentRendererMarkdown.4d1adc87.js";import"./index.a6ef77ff.js";import"./preview.ed3c5a8f.js";import"./ContentQuery.f4a10327.js";import"./asyncData.c60e0214.js";import"./utils.3c075076.js";const j=a=>(L("data-v-89494b79"),a=a(),W(),a),U={key:0,class:"navigation-buttons-container"},X={key:0,class:"button left"},Y={class:"link-text"},Z=j(()=>l("i",{class:"fa-solid fa-caret-left"},null,-1)),G={key:1,class:"button right"},H={class:"link-text"},J=j(()=>l("i",{class:"fa-solid fa-caret-right"},null,-1)),Q=O({__name:"BottomNavigation",setup(a){const t=M(),c=V().pages;let o=0;c.forEach((_,m)=>{_._path===t.path&&(o=m)});const f=o>0?c[o-1]:null,n=o<c.length+1?c[o+1]:null;return(_,m)=>{var s;const u=T;return d(f)!=null||d(n)!=null?(h(),v("div",U,[d(f)!=null?(h(),v("div",X,[y(u,{to:d(f)._path},{default:x(()=>{var r;return[l("div",Y,[Z,$(" Previous "),l("h3",null,w((r=d(f))==null?void 0:r.title),1)])]}),_:1},8,["to"])])):k("",!0),d(n)!=null?(h(),v("div",G,[y(u,{to:(s=d(n))==null?void 0:s._path},{default:x(()=>{var r;return[l("div",H,[$(" Next "),J,l("h3",null,w((r=d(n))==null?void 0:r.title),1)])]}),_:1},8,["to"])])):k("",!0)])):k("",!0)}}});const tt=I(Q,[["__scopeId","data-v-89494b79"]]),et=O({__name:"Toc",props:{value:{}},setup(a){let t;const e=S(""),c=S(!1);function o(){c.value=!c.value}function f(){c.value=!1}return R(()=>{t=new IntersectionObserver(m,{root:document,rootMargin:"-75px"});const n={},_={};document.querySelectorAll(".toc-root > h2[id], .toc-root > h3[id]").forEach((s,r)=>{const g=s.getAttribute("id");_[g]=r}),document.querySelectorAll(".toc-root > *").forEach(s=>t.observe(s));function m(s){const r=[];for(const p of s){const i=u(p.target);i&&r.push({id:i,entry:p})}for(const{entry:p,id:i}of r)n[i]||(n[i]=0),p.isIntersecting?n[i]+=1:n[i]-=1;for(const p of Object.keys(n))n[p]<=0&&delete n[p];const g=Object.keys(n);g.sort((p,i)=>_[p]-_[i]),e.value=g[0]||""}function u(s){for(;_[s.getAttribute("id")||""]===void 0;){if(!s.previousElementSibling)return null;s=s.previousElementSibling}return s.getAttribute("id")}}),z(()=>{t.disconnect()}),(n,_)=>{const m=T;return h(),v("nav",null,[l("h4",null,[l("a",{onClick:o},"On This Page")]),l("ul",{class:P({active:d(c)})},[(h(!0),v(N,null,B(n.value.links,u=>(h(),v("li",{key:u.id},[y(m,{to:`#${u.id}`,class:P({active:u.id===d(e)}),onClick:f},{default:x(()=>[$(w(u.text),1)]),_:2},1032,["to","class"]),l("ul",null,[(h(!0),v(N,null,B(u.children,s=>(h(),v("li",{key:s.id},[y(m,{to:`#${s.id}`,class:P({active:s.id===d(e)}),onClick:f},{default:x(()=>[$(w(s.text),1)]),_:2},1032,["to","class"])]))),128))])]))),128)),l("li",null,[l("a",{class:"return-to-top",href:"#",onClick:f},"Return to top")])],2)])}}});const ot=I(et,[["__scopeId","data-v-bf4f74b1"]]);function nt(a,t){const e=new Map;for(let o=0;o<a.pages.length;o++)e.set(t[o]._path,t[o]);const c=t;for(let o=0;o<a.pages.length;o++)c[o]=e.get(`${a._path}/${a.pages[o]}`);return c}function st(a,t,e="title",c="asc"){return c==="desc"?ct(a,t,e):at(a,t,e)}function at(a,t,e){return a[e]<t[e]?-1:a[e]>t[e]?1:0}function ct(a,t,e){return t[e]<a[e]?-1:t[e]>a[e]?1:0}const rt={class:"container"},_t={class:"content"},it={__name:"[...content-page]",async setup(a){let t,e;const c=M(),o=`/${c.path.split("/")[1]}`,f=V(),n=([t,e]=E(()=>A(o).where({_path:{$eq:o}}).only(["_path","title","pages","sort"]).findOne()),t=await t,e(),t);let _=([t,e]=E(()=>A(o).where({_path:{$ne:o}}).only(["_path","title"]).find()),t=await t,e(),t);n.pages?_=nt(n,_):n.sort?_.sort((s,r)=>st(s,r,n.sort.by,n.sort.order)):_.sort();const m=o==="/"?[n]:[n,..._];f.setPages(m);const u=c.path!=="/"&&c.path.endsWith("/");return R(async()=>{if(u){const{path:s,query:r,hash:g}=c,i={path:s.replace(/\/+$/,""),query:r,hash:g};await F(i,{replace:!0})}}),(s,r)=>{const g=D,p=tt,i=ot,C=K;return d(u)?k("",!0):(h(),q(C,{key:0},{default:x(({doc:b})=>[l("div",rt,[l("div",_t,[y(g,{value:b,class:P(["toc-root",{"no-toc":b.body.toc.links<=0}])},null,8,["value","class"]),y(p)]),b.body.toc.links.length>0?(h(),q(i,{key:0,value:b.body.toc},null,8,["value"])):k("",!0)])]),"not-found":x(()=>[y(C,{path:"/not-found",head:!1})]),_:1}))}}},bt=I(it,[["__scopeId","data-v-4240d3a7"]]);export{bt as default};