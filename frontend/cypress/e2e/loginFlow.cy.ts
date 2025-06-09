// https://on.cypress.io/api


import { loginAsAdmin } from '../support/helper.cy.ts'

describe('Login flows', () => {
  const baseUrl = 'http://localhost:5173'

  it('should fail login with invalid credentials', () => {
    cy.visit(`${baseUrl}/`)

    cy.get('[data-cy=loginEmail]').type('wrong@example.com')
    cy.get('[data-cy=loginPassword]').type('wrongpassword')
    cy.get('[data-cy=loginSubmit]').click()
    cy.get('.p-toast-message').should('exist').should('have.class', 'p-toast-message-error')
  })

  it('should log in successfully with valid credentials', () => {
    loginAsAdmin()
  })
})
