import v from"./Header.70cac991.js";import S from"./Side.a0e8229e.js";import w from"./Footer.bd76959e.js";import{i as g,f as i,s as c,g as p,o as n,c as a,b as _,a6 as x,a as l,r as d,_ as $}from"./entry.a3094e82.js";import{q as m}from"./query.ae01de8b.js";import{a as k}from"./applyOrder.347713b5.js";import{u as q}from"./sectionStore.f616c452.js";import"./nuxt-link.e46f0f23.js";import"./pageStore.e2fee975.js";import"./preview.2c12b7d4.js";const B={key:0},C={class:"full"},b={key:1,class:"content"},N=g({__name:"default",async setup(O){let e,o;const u=q(),r=([e,o]=i(()=>m().where({_path:"/"}).only(["sections"]).findOne()),e=await e,o(),e),s=([e,o]=i(()=>m("/").only(["_path","title","_dir"]).where({_dir:{$eq:""},_path:{$in:r.sections.map(t=>c(t))}}).find()),e=await e,o(),e);for(const t of s)t._path=p(t._path);return k(s,t=>c(p(r.sections[t]))),u.setSections(s),(t,V)=>{const f=v,h=S,y=w;return n(),a("div",null,[_(f),(t._.provides[x]||t.$route).path==="/"?(n(),a("div",B,[l("article",C,[d(t.$slots,"default",{},void 0,!0)])])):(n(),a("div",b,[_(h,{class:"side"}),l("article",null,[d(t.$slots,"default",{},void 0,!0)])])),_(y)])}}});const z=$(N,[["__scopeId","data-v-5e897649"]]);export{z as default};