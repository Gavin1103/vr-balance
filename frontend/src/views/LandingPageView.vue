<script setup lang="ts">
// No script logic needed unless you want interactivity
</script>

<template>
  <div class="landing-page">
    <section class="hero">
      <h1>Welcome to VRBalance</h1>
      <p>
        VRBalance is a virtual reality physiotherapy application that helps track your balance and posture during exercises like squats, lunges, and glute bridges.
      </p>
      <a href="/login" class="cta-button">Go to App</a>
    </section>

    <section class="features">
      <h2>Key Features</h2>
      <ul>
        <li>🧍‍♂️ Real-time balance & headsway tracking</li>
        <li>📈 Detailed analytics after each session</li>
        <li>🧠 Personalized feedback for better posture</li>
        <li>💻 Secure online dashboard to review progress</li>
      </ul>
    </section>

    <section class="media">
      <h2>Preview</h2>
      <!-- Replace with your screenshot or video -->
      <img src="/preview.png" alt="VRBalance Screenshot" class="preview" />
    </section>

    <section class="info">
      <p>Available for Meta Quest headsets</p>
      <a href="/policy">Privacy Policy</a> | <a href="mailto:contact@vrbalance.com">Contact Us</a>
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