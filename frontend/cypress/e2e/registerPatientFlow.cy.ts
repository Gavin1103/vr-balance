// https://on.cypress.io/api
import { loginAsAdmin } from '../support/helper.cy.ts'
import type { RegisterPatientDTO } from '../../src/request/User/RegisterPatientDTO.ts'

describe('Register patient flows', () => {
  const apiUrl = 'http://localhost:8080'

  beforeEach(() => {
    cy.request('DELETE', `${apiUrl}/api/test/public/cleanup-cypress-test-user/emailCypress@vrbalance.com`)
  })

  it('registers a patient with valid info', () => {
    loginAsAdmin()
    fillRegisterForm({
      firstName: 'firstNameCypress',
      lastName: 'lastNameCypress',
      username: 'usernameCypress',
      email: 'emailCypress@vrbalance.com',
      birthDate: '2025-03-11',
    })

    cy.get('.p-toast-message').should('exist').should('have.class', 'p-toast-message-success')
  })

  it('shows error when email already exists', () => {
    loginAsAdmin()
    fillRegisterForm({
      firstName: 'firstNameCypress',
      lastName: 'lastNameCypress',
      username: 'usernameCypress',
      email: 'admin@vrbalance.com',
      birthDate: '2025-03-11',
    })

    cy.get('.p-toast-message').should('exist').should('have.class', 'p-toast-message-error')
  })

  it('shows error when username already exists', () => {
    loginAsAdmin()
    fillRegisterForm({
      firstName: 'firstNameCypress',
      lastName: 'lastNameCypress',
      username: 'admin',
      email: 'emailCypress@vrbalance.com',
      birthDate: '2025-03-11',
    })

    cy.get('.p-toast-message').should('exist').should('have.class', 'p-toast-message-error')
  })
})

function fillRegisterForm(request: RegisterPatientDTO) {
  cy.get('[data-cy=registerPatientButton]').click()
  cy.get('[data-cy=registerPatientForm]').should('be.visible')

  cy.get('[data-cy=firstName]').type(request.firstName)
  cy.get('[data-cy=lastName]').type(request.lastName)
  cy.get('[data-cy=username]').type(request.username)
  cy.get('[data-cy=email]').type(request.email)
  cy.get('[data-cy=birthDate]').type(request.birthDate)
  cy.get('[data-cy=submit]').click()
}


