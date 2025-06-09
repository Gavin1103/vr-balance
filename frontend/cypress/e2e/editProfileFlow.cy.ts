// https://on.cypress.io/api
import { loginAsAdmin } from '../support/helper.cy.ts'
import type { EditUserDTO } from '../../src/request/User/EditUserDTO.ts'

describe('Edit profile flows', () => {

  it('should allow a user to edit their profile successfully', () => {
    // --- GIVEN ---
    // De gebruiker is ingelogd en bevindt zich op de profielpagina
    loginAsAdmin()
    cy.get('[data-cy=profileButton]').click()
    cy.url().should('include', '/profile')

    // --- WHEN ---
    // De gebruiker activeert de edit-modus en wijzigt zijn profielgegevens
    cy.get('[data-cy=editProfileButton]').click()
    fillEditProfileForm({
      email: 'admin@vrbalance.com',
      username: 'admin',
      birthDate: '2025-01-01',
      firstName: 'newFirstName',
      lastName: 'newLastName',
      phoneNumber: '06000000000',
    })
    cy.get('[data-cy=submitEditProfileButton]').click()

    // --- THEN ---
    // De applicatie toont een succesmelding en deactiveerd de velden
    cy.get('.p-toast-message').should('exist').should('have.class', 'p-toast-message-success')

    cy.get('#email').should('be.disabled')
    cy.get('#username').should('be.disabled')
    cy.get('#birthDate').should('be.disabled')
    cy.get('#firstName').should('be.disabled')
    cy.get('#lastName').should('be.disabled')
    cy.get('#phoneNumber').should('be.disabled')
  })

  it('Using an email that is already in use', () => {
    loginAsAdmin()

    cy.get('[data-cy=profileButton]').click()
    cy.url().should('include', '/profile')
    cy.get('[data-cy=editProfileButton]').click()

    fillEditProfileForm({
      email: 'gavin@vrbalance.com',
      username: 'admin',
      birthDate: '2025-01-01',
      firstName: 'newFirstName',
      lastName: 'newLastName',
      phoneNumber: '06000000000',
    })

    cy.get('[data-cy=submitEditProfileButton]').click()
    cy.get('.p-toast-message').should('exist').should('have.class', 'p-toast-message-error')
  })

  it('Using an username that is already in use', () => {
    loginAsAdmin()

    cy.get('[data-cy=profileButton]').click()
    cy.url().should('include', '/profile')
    cy.get('[data-cy=editProfileButton]').click()

    fillEditProfileForm({
      email: 'admin@vrbalance.com',
      username: 'gavin',
      birthDate: '2025-01-01',
      firstName: 'newFirstName',
      lastName: 'newLastName',
      phoneNumber: '06000000000',
    })

    cy.get('[data-cy=submitEditProfileButton]').click()
    cy.get('.p-toast-message').should('exist').should('have.class', 'p-toast-message-error')
  })
})

function fillEditProfileForm(data: EditUserDTO) {
  cy.get('[data-cy=submitEditProfileButton]').should('be.visible');

  cy.get('#email').should('not.be.disabled').clear().type(data.email);
  cy.get('#username').should('not.be.disabled').clear().type(data.username);
  cy.get('#birthDate').should('not.be.disabled').clear().type(data.birthDate);
  cy.get('#firstName').should('not.be.disabled').clear().type(data.firstName);
  cy.get('#lastName').should('not.be.disabled').clear().type(data.lastName);
  cy.get('#phoneNumber').should('not.be.disabled').clear().type(data.phoneNumber);
}
