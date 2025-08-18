import {KeycloakService} from 'keycloak-angular';

export const initializeKeycloak = (keycloak: KeycloakService) => {
  return () => keycloak.init({
    config: {
      clientId: "my_frontend_app",
      realm: 'codestar',
      url: "http://192.168.25.178:8080",
    },
    initOptions: {
      onLoad: 'check-sso',
      checkLoginIframe: true,
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-login.html',
    },
    bearerExcludedUrls: ['/assets']
  })
}
