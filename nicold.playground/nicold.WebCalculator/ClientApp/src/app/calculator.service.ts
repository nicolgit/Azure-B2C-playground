export class CalculatorService {

  constructor() {
    this.parameter1 = 12;
    this.parameter2 = 34;
    this.isAuthenticated = false;
    this.username = "";

    this.applicationConfig = {
      clientID: 'c07391de-3205-4496-a704-4607b18b64f9',
      authority: "https://login.microsoftonline.com/tfp/nicolb2c.onmicrosoft.com/B2C_1_signin-default",
      b2cScopes: ["https://nicolb2c.onmicrosoft.com/WebCalculator/user_impersonation"],
    };
  }
  applicationConfig: any;

  isAuthenticated: boolean

  parameter1: number;
  parameter2: number;
  result: number;
}
