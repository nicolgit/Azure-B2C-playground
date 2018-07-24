import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CalculatorService } from '../calculator.service';
import * as Msal from 'msal';

@Component({
  selector: 'app-counter-component',
  templateUrl: './calculator.component.html',
  providers: [CalculatorService],
})

export class CalculatorComponent {
  private logger: Msal.Logger;
  private clientApplication: Msal.UserAgentApplication;

  constructor(private calculatorService: CalculatorService) {
    
    this.logger = new Msal.Logger(this.loggerCallback, { level: Msal.LogLevel.Verbose });
    this.clientApplication = new Msal.UserAgentApplication(calculatorService.applicationConfig.clientID, calculatorService.applicationConfig.authority, this.authCallback);
  }

  get param1(): number {
    return this.calculatorService.parameter1;
  }
  set param1(val: number) {
    this.calculatorService.parameter1 = val;
  }

  get param2(): number {
    return this.calculatorService.parameter2;
  }
  set param2(val: number) {
    this.calculatorService.parameter2 = val;
  }

  username: string;
  
  get result(): string {
    if (this.calculatorService.isAuthenticated) {
      return this.calculatorService.result.toString();
    }
    else {
      return "";
    }
  }

  public authenticate() {
    this.clientApplication.loginPopup(this.calculatorService.applicationConfig.b2cScopes).then(function (idToken) {
      this.clientApplication.acquireTokenSilent(this.calculatorService.applicationConfig.b2cScopes).then(function (accessToken) {
        this.updateUI();
      }, function (error) {
        this.clientApplication.acquireTokenPopup(this.calculatorService.applicationConfig.b2cScopes).then(function (accessToken) {
          this.updateUI();
        }, function (error) {
          console.log("Error acquiring the popup:\n" + error);
        });
      })
    }, function (error) {
      console.log("Error during login:\n" + error);
    });
  }

  private authCallback(errorDesc, token, error, tokenType) {
    if (token) {
      
    }
    else {
      console.log(error + ":" + errorDesc); 
    }
  }

  private updateUI() {
    this.username = this.clientApplication.getUser().name;
  }
  

  private loggerCallback(logLevel, message, piiLoggingEnabled) {
    console.log(message);
  }
}
