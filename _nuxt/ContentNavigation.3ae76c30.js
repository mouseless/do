import{u as f}from"./asyncData.c60e0214.js";import{s as v,_ as g,a as d,v as l,i as h,x as _,h as y,t as r}from"./entry.42953906.js";import{q as w,h as p,e as C,j as P}from"./query.060b90cc.js";import{_ as $}from"./nuxt-link.0ac70c5f.js";import{w as c,s as N,u as j}from"./utils.3c075076.js";import{u as T}from"./preview.ed3c5a8f.js";const x=async e=>{const{content:t}=v().public;typeof(e==null?void 0:e.params)!="function"&&(e=w(e));const a=e.params(),s=t.experimental.stripQueryParameters?c(`/navigation/${`${p(a)}.${t.integrity}`}/${C(a)}.json`):c(`/navigation/${p(a)}.${t.integrity}.json`);if(N())return(await g(()=>import("./client-db.4a33f4fb.js"),["./client-db.4a33f4fb.js","./entry.42953906.js","./entry.7dae836b.css","./query.060b90cc.js","./utils.3c075076.js","./preview.ed3c5a8f.js","./index.a6ef77ff.js"],import.meta.url).then(o=>o.generateNavigation))(a);const n=await $fetch(s,{method:"GET",responseType:"json",params:t.experimental.stripQueryParameters?void 0:{_params:P(a),previewToken:T().getPreviewToken()}});if(typeof n=="string"&&n.startsWith("<!DOCTYPE html>"))throw new Error("Not found");return n},R=d({name:"ContentNavigation",props:{query:{type:Object,required:!1,default:void 0}},async setup(e){const{query:t}=l(e),a=h(()=>{var n;return typeof((n=t.value)==null?void 0:n.params)=="function"?t.value.params():t.value});if(!a.value&&_("dd-navigation").value){const{navigation:n}=j();return{navigation:n}}const{data:s}=await f(`content-navigation-${p(a.value)}`,()=>x(a.value));return{navigation:s}},render(e){const t=y(),{navigation:a}=e,s=o=>r($,{to:o._path},()=>o.title),n=(o,u)=>r("ul",u?{"data-level":u}:null,o.map(i=>i.children?r("li",null,[s(i),n(i.children,u+1)]):r("li",null,s(i)))),m=o=>n(o,0);return t!=null&&t.default?t.default({navigation:a,...this.$attrs}):m(a)}});export{R as default};