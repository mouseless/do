<template>
  <nav>
    <h4><a @click="toggle">Pages</a></h4>
    <ul :class="{ active: shown }">
      <li>
        <NuxtLink
          v-for="menu in menus"
          :key="menu.title"
          :to="menu._path"
          :class="{ active: menu._path == $route.path }"
          @click="close"
        >
          {{ menu.title }}
        </NuxtLink>
      </li>
    </ul>
  </nav>
</template>
<script lang="ts" setup>
import { watch, ref } from "#imports";
import { usePageStore } from "~/store/pageStore";

const store = usePageStore();

const shown = ref<boolean>(false);

const menus: any = ref(store.pages);

function close() { shown.value = false; }
function toggle() { shown.value = !shown.value; }

watch(usePageStore(), () => {
  menus.value = store.pages;
});
</script>
<style lang="scss" scoped>

$a-padding-lr: 1em;
$a-padding-tb: 0.5em;

nav {
  position: sticky;
  align-self: start;
  top: 1.5em;
  margin-top: 2.5em;

  h4 {
    display: none;
  }

  ul {
    margin: 0;
    padding: 0;

    li {
      margin: 0;
      list-style: none;

      a {
        font-size: 1em;
        display: block;
        text-decoration: none;
        color: $color-fg-second;
        border-radius: calc($border-radius / 3);
        padding: $a-padding-tb $a-padding-lr;

        &:hover {
          color: $color-brand;
        }

        &.active {
          background-color: $color-bg-nav-active;
        }
      }
    }
  }
}

@media (max-width: $width-page-l) {
  nav {
    margin-top: 3.5em;
  }
}

@media (max-width: $width-page-m) {
  nav {
    margin-top: 0;
    width: 50%;
    top: 0;

    h4 {
      display: block;
      text-transform: uppercase;
      font-size: 0.9em;

      a {
        padding-left: 0;

        &:hover {
          color: $color-brand;
        }
      }
    }

    ul {
      display: none;
      @include box;
      background-color: $color-bg-nav;
      position: absolute;
      width: 200%;
      padding: calc($border-radius - $a-padding-tb) calc($border-radius - $a-padding-lr);
      box-sizing: border-box;

      &.active {
        display: block;
      }

      li a {
        &.active {
          background-color: $color-bg-third;
        }
      }
    }
  }
}

@media (max-width: $width-page-s) {
  nav {
    margin-bottom: -3.7em;
  }
}
</style>
