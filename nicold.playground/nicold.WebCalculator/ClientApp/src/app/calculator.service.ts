import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
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
    this.parameter2 = 4;
    this.isAuthenticated = false;
    this.isLoading = false;
    this.username = "--";
    this.result = "";

    this.applicationConfig = new ApplicationConfig();
    this.applicationConfig.clientID = 'c07391de-3205-4496-a704-4607b18b64f9';
    this.applicationConfig.authority = "https://login.microsoftonline.com/tfp/nicolb2c.onmicrosoft.com/B2C_1_signin-default";
    this.applicationConfig.b2cScopes = ["https://nicolb2c.onmicrosoft.com/WebCalculator/user_impersonation"];

    this.applicationConfig.calculatorApiEndopoint = "https://nicolapicalculator.azurewebsites.net/api/calc/";
    this.applicationConfig.scientificCalculatorApiEndopoint = "https://nicolapiscientificcalculator.azurewebsites.net/api/scientificcalc/";
  }

  applicationConfig: ApplicationConfig;

  isAuthenticated: boolean;
  isLoading: boolean;
  parameter1: number;
  parameter2: number;
  result: string;
  username: string;
  accessToken: string;

  public getCallSum() {
    return this.callCalculator("sum");
  }

  public getCallSubtract() {
    return this.callCalculator("subtract");
  }

  public getCallMultiply() {
    return this.callCalculator("multiply");
  }
  public getCallSplit() {
    return this.callCalculator("split");
  }

  public getCallPower() {
    return this.callScientificCalculator("power");
  }

  public getCallPercentage() {
    return this.callScientificCalculator("percentage");
  }

  public getCallFactorial() {
    return this.callScientificCalculator("factorial");
  }

  private callCalculator(operation: string) {
    this.isLoading = true;
    this.result = "";

    let httpheaders = new HttpHeaders()
      .set('Authorization', "Bearer " + this.accessToken);

    let httpparams = new HttpParams()
      .set('param1', this.parameter1.toString())
      .set('param2', this.parameter2.toString());

    let url = "" + this.applicationConfig.calculatorApiEndopoint + operation;

    return this.http.get(url, { responseType: 'text', headers: httpheaders, params: httpparams });
  }

  private callScientificCalculator(operation: string) {
    this.isLoading = true;
    this.result = "";

    let httpheaders = new HttpHeaders()
      .set('Authorization', "Bearer " + this.accessToken);

    let httpparams = new HttpParams()
      .set('param1', this.parameter1.toString())
      .set('param2', this.parameter2.toString());

    let url = "" + this.applicationConfig.scientificCalculatorApiEndopoint + operation;

    return this.http.get(url, { responseType: 'text', headers: httpheaders, params: httpparams });
  }
}
