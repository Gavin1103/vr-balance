<script setup lang="ts">
import router from '@/router/BaseRouter.ts'
import RegisterPatientComponent from '@/components/RegisterPatientComponent.vue'

async function clickProfileImage(): Promise<void> {
  console.log('clickProfileImage')
  await router.push({ name: 'profile' }).then(() => {
    window.location.reload()
  })
}
</script>
<template>
  <section class="top-bar">
    <div class="top-bar-left">
      <input type="text" placeholder="Search all" class="search-input" />
    </div>
    <div class="top-bar-right">
      <div @click="clickProfileImage" class="profile-img-container" data-cy="profileButton">
        <img src="../assets/img/profile-picture-blank.webp" alt="profile-img" />
      </div>
    </div>
  </section>
</template>

<style scoped>
.top-bar {
  margin: 0 0 50px 0;
  width: 100%;
  min-height: 100px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  display: flex;

  .top-bar-left {
    width: 50%;
    display: flex;
    align-items: center;

    .search-input {
      margin: 0 0 0 10px;
      padding: 1rem 1rem;
      width: 100%;
      max-width: 400px;
      border: 1px solid lightgrey;
      border-radius: 0.5rem;
      font-size: 1rem;
    }
  }

  .top-bar-right {
    width: 50%;
    display: flex;
    align-items: center;
    justify-content: flex-end;

    .profile-img-container {
      height: 60px;
      width: 60px;
      border-radius: 50%;
      margin: 0 20px 0 0;
      display: flex;
      justify-content: center;
      align-items: center;
      border: 1px solid lightgrey;
      overflow: hidden;
      transition: transform 0.3s ease;

      &:hover {
        transform: scale(1.3);
        cursor: pointer;
      }

      img {
        height: 100%;
        border-radius: 50%;
      }
    }
  }
}
</style>
