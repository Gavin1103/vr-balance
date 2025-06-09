export class CookieService {
  readonly accessTokenAlias: string = 'access_token';
  // readonly refreshTokenAlias: string = 'refresh_token';

  /**
   * Set the access token cookie
   * @param {string} token - Access token
   */
  // setTokenCookies(token: string, refreshToken: string): void {
  public setTokenCookies(token: string): void {
    this.setCookie(this.accessTokenAlias, token, 1);
    // this.setCookie(this.refreshTokenAlias, refreshToken, 1);
  }

  /**
   * Remove the access token and refresh token cookies
   */
  public removeTokenCookies(): void {
    this.deleteCookie(this.accessTokenAlias);
    // this.deleteCookie(this.refreshTokenAlias);
  }

  public tokenExists(): boolean {
    return this.getCookie(this.accessTokenAlias) !== null;
  }

  /**
   * Get a cookie by name
   * @param {string} name - Name of the cookie
   * @returns {string | null} - Value of the cookie or null if not found
   */
  public getCookie(name: string): string | null {
    const nameEQ = name + "=";
    const cookiesArray = document.cookie.split(';');
    for (let i = 0; i < cookiesArray.length; i++) {
      const cookie = cookiesArray[i].trim();
      if (cookie.indexOf(nameEQ) === 0) {
        return decodeURIComponent(cookie.substring(nameEQ.length, cookie.length));
      }
    }
    return null;
  }

  /**
   * Set a cookie
   * @param {string} name - Name of the cookie
   * @param {string} value - Value of the cookie
   * @param {number | string} days - Expiration in days (if number) or a specific expiration date string (if string)
   * @param {string} path - Path where the cookie is valid, defaults to root "/"
   */
  private setCookie(name: string, value: string, days: number | string, path: string = '/'): void {
    let expires = "";
    if (typeof days === 'number') {
      const date = new Date();
      date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
      expires = "; expires=" + date.toUTCString();
    } else {
      expires = "; expires=" + days;
    }

    document.cookie = `${name}=${encodeURIComponent(value)}${expires}; path=${path}`;
  }
  /**
   * Delete a cookie by setting its expiration to a past date
   * @param {string} name - Name of the cookie
   * @param {string} path - Path of the cookie, defaults to root "/"
   */
  private deleteCookie(name: string, path: string = '/'): void {
    document.cookie = `${name}=; Expires=Thu, 01 Jan 1970 00:00:01 GMT; path=${path}`;
  }
}
