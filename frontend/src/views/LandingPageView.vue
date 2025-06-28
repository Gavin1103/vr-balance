<script setup lang="ts">
</script>

<template>
  <div class="landing-page">
    <section class="hero">
      <h1>Welcome to</h1>
      <h1 style="font-size: 50px; margin-top: -30px;">Motion: Virtual Physiotherapy</h1>
      <p>
        Motion: Virtual Physiotherapy is a VR application designed to assist users with lower back pain by improving their balance, core strength and flexibility.      
      </p>
      <a href="/login" class="cta-button">Login and see how well you're doing</a>
    </section>

    <section class="features">
      <h2>Key Features</h2>
      <ul>
        <li>Real-time balance & headsway tracking</li>
        <li>Detailed analytics after each session</li>
        <li>Personalized feedback and reviews from a licensed physiotherapist</li>
        <li>Fun minigames</li>
      </ul>
    </section>

    <section class="media">
      <h2>Preview</h2>
      <img src="/preview.png" alt="Motion Screenshot" class="preview" />
    </section>

    <section class="info">
      <p>Available for Meta Quest headsets</p>
      <a href="/policy">Privacy Policy</a> | <a href="mailto:amine.el.hammdaoui@hva.nl">Contact Lead Dev</a>
    </section>
  </div>
</template>

<style scoped>
.landing-page {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem;
  font-family: sans-serif;
  color: #333;
  line-height: 1.6;
}

.hero {
  text-align: center;
  margin-bottom: 2rem;
}

.hero h1 {
  font-size: 2.5rem;
  margin-bottom: 1rem;
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

.features ul {
  list-style: none;
  padding: 0;
}

.features li {
  padding: 0.5rem 0;
}

.media .preview {
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.info {
  text-align: center;
  margin-top: 2rem;
}

.info a {
  color: #7393b3;
  margin: 0 0.5rem;
}
</style>