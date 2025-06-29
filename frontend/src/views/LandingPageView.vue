<script setup lang="ts">
import { useAuthenticationStore } from '@/stores/AuthenticationStore.ts'
import { onMounted } from 'vue'

const authentication = useAuthenticationStore()

onMounted(async () => {
  await authentication.checkAuthorization()
})
</script>

<template>
  <div class="landing-page">
    <section class="hero">
      <h1>Virtual Physiotherapy</h1>
      <p>
        Welcome to Motion – a VR physiotherapy experience that helps relieve lower back pain by
        improving your balance, core strength, and flexibility.
      </p>
      <a v-if="!authentication.isLoggedIn" href="/login" class="cta-button">
        Login to view your progress
      </a>
    </section>

    <section class="features">
      <h2>Key Features</h2>
      <ul>
        <li>- Real-time balance & headsway tracking</li>
        <li>- Detailed analytics after each session</li>
        <li>- Personalized feedback and reviews from a licensed physiotherapist</li>
        <li>- Fun mini games</li>
      </ul>
    </section>

    <section class="media">
      <h2>Preview</h2>
      <img src="/preview.png" alt="Motion Screenshot" class="preview" />
      <img src="/preview_2.png" alt="Motion Screenshot" class="preview" />
      <img src="/preview_3.png" alt="Motion Screenshot" class="preview" />
    </section>

    <section class="info">
      <p>Available for Meta Quest headsets</p>
      <a href="/policy">Privacy Policy</a> |
      <a href="mailto:amine.el.hammdaoui@hva.nl">Contact developers</a>
    </section>
  </div>
</template>

<style scoped>
.landing-page {
  max-width: 800px;
  margin: 50px auto 0 auto;
  padding: 0 0 50px 0;
  font-family: sans-serif;
  color: #333;
  line-height: 1.6;

  .hero {
    text-align: center;
    margin-bottom: 2rem;

    h1 {
      font-size: 2.5rem;
    }

    .cta-button {
      display: inline-block;
      margin-top: 1rem;
      padding: 0.75rem 1.5rem;
      background-color: #7393b3;
      color: white;
      text-decoration: none;
      border-radius: 4px;
      font-weight: bold;
    }
  }

  .features {
    ul {
      list-style: none;
      padding: 0;
    }
  }

  .media {
    margin: 20px 0 0 0;

    .preview {
      width: 100%;
      border-radius: 8px;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    }
  }

  .info {
    text-align: center;
    margin-top: 2rem;

    a {
      color: #7393b3;
      margin: 0 0.5rem;
    }
  }
}
</style>
