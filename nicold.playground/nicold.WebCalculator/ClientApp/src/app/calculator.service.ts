import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/add/operator/map';

export class ApplicationConfig {
  clientID: string;
  authority: string;
  b2cScopes: [string];

  calculatorApiEndopoint: string;
  scientificCalculatorApiEndopoint: string;
};

@Injectable()
export class CalculatorService {

  constructor(private http: HttpClient)  {
    this.parameter1 = 12;
    this.parameter2 = 34;
    this.isAuthenticated = false;
    this.username = "prova";
    this.result = 0;

    this.applicationConfig = new ApplicationConfig();
    this.applicationConfig.clientID = 'c07391de-3205-4496-a704-4607b18b64f9';
    this.applicationConfig.authority = "https://login.microsoftonline.com/tfp/nicolb2c.onmicrosoft.com/B2C_1_signin-default";
    this.applicationConfig.b2cScopes = ["https://nicolb2c.onmicrosoft.com/WebCalculator/user_impersonation"];

    this.applicationConfig.calculatorApiEndopoint = "https://nicolapicalculator.azurewebsites.net/api/calc/";
    this.applicationConfig.scientificCalculatorApiEndopoint = "https://nicolapiscientificcalculator.azurewebsites.net/api/calc/";
  }

  applicationConfig: ApplicationConfig;

  isAuthenticated: boolean;
  parameter1: number;
  parameter2: number;
  result: number;
  username: string;
  accessToken: string;

  public getCallSum() {
    return this.callCalculator("sum")
  }


  private callCalculator(operation: string) {

    let httpOptions = {
      headers: new HttpHeaders({
        'Authorization': "Bearer " + this.accessToken
      }),
      responseType: 'text'
    };
    
    let url = "" + this.applicationConfig.calculatorApiEndopoint + operation + "?param1=" + this.parameter1 + "&param2=" + this.parameter2;
    
    return this.http.get(url, {httpOptions);
  }
}
