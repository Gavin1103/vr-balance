// https://on.cypress.io/api

/// <reference types="cypress" />

export function loginAsAdmin() {
  cy.visit('http://localhost:5173')

  cy.get('[data-cy=loginEmail]').type("admin@vrbalance.com")
  cy.get('[data-cy=loginPassword]').type("admin")
  cy.get('[data-cy=loginSubmit]').click()
  cy.url().should('include', '/dashboard')
}
