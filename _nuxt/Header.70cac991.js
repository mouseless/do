import{_ as b}from"./nuxt-link.e46f0f23.js";import{i as k,u as x,y as C,m as S,o as r,c as d,a as s,b as c,w as l,n as p,j as _,F as y,B as w,v as B,d as I,t as N,p as L,e as R,_ as V}from"./entry.a3094e82.js";import{u as $}from"./sectionStore.f616c452.js";const a=t=>(L("data-v-049e3959"),t=t(),R(),t),F={class:"top"},H={class:"logo"},j=a(()=>s("img",{class:"do logo"},null,-1)),z=a(()=>s("i",{class:"fa-solid fa-bars"},null,-1)),D=[z],E=a(()=>s("i",{class:"fa-solid fa-close"},null,-1)),T=[E],U=a(()=>s("i",{class:"fa-brands fa-github"},null,-1)),q=k({__name:"Header",setup(t){const f=B(),h=x(),m=$(),o=C(!1),g=S(()=>`/${h.path.split("/")[1]}/`),v={...m.sections};function i(){o.value=!o.value}function u(){o.value=!1}return(A,G)=>{const n=b;return r(),d("div",F,[s("header",null,[s("div",H,[c(n,{to:"/"},{default:l(()=>[j]),_:1})]),s("a",{class:"bars",onClick:i},D),s("nav",{class:p({active:_(o)})},[s("a",{class:"close",onClick:i},T),(r(),d(y,null,w(v,e=>c(n,{key:e.title,to:e._path,class:p({active:e._path===_(g)}),onClick:u},{default:l(()=>[I(N(e.title),1)]),_:2},1032,["to","class"])),64)),c(n,{to:`https://github.com${_(f).public.githubURL}`,target:"_blank",onClick:u},{default:l(()=>[U]),_:1},8,["to"])],2)])])}}});const O=V(q,[["__scopeId","data-v-049e3959"]]);export{O as default};