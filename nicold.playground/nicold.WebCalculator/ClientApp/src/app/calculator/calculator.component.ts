import { Component, Input, Output, EventEmitter, ChangeDetectorRef, ApplicationRef, NgZone, ChangeDetectionStrategy } from '@angular/core';
import { CalculatorService } from '../calculator.service';
import * as Msal from 'msal';

@Component({
  selector: 'app-counter-component',
  templateUrl: './calculator.component.html',
  providers: [CalculatorService],
  changeDetection: ChangeDetectionStrategy.Default
})

export class CalculatorComponent {
  private logger: Msal.Logger;
  private clientApplication: Msal.UserAgentApplication;

  constructor(private calculatorService: CalculatorService, private cd: ChangeDetectorRef) {
    
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

  get username(): string {
    return this.calculatorService.username;
  }
  set username(val: string) {
    this.calculatorService.username = val;
  }

  get isAuthenticated(): boolean {
    return this.calculatorService.isAuthenticated;
  }
  set isAuthenticated(val: boolean) {
    this.calculatorService.isAuthenticated = val;
  }

  
  get result(): string {
    if (this.calculatorService.isAuthenticated) {
      return this.calculatorService.result.toString();
    }
    else {
      return "";
    }
  }


  public authenticate() {
    var _this = this;

    console.log("begin loginPopup");
    _this.clientApplication.loginPopup(_this.calculatorService.applicationConfig.b2cScopes).then(function (idToken) {
      console.log("begin acquireTokenSilent");
      _this.clientApplication.acquireTokenSilent(_this.calculatorService.applicationConfig.b2cScopes).then(function (accessToken) {
        _this.calculatorService.accessToken = accessToken;
        _this.updateUI();
      }, function (error) {
        console.log("ERROR begin acquireTokenPopup");
        _this.clientApplication.acquireTokenPopup(_this.calculatorService.applicationConfig.b2cScopes).then(function (accessToken) {
        _this.updateUI();
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
      console.log("Token:" + token);
    }
    else {
      console.log(error + ":" + errorDesc); 
    }
  }

  private updateUI() {
    this.username = this.clientApplication.getUser().userIdentifier;
    this.isAuthenticated = true;
  }
  
  private loggerCallback(logLevel, message, piiLoggingEnabled) {
    console.log(message);
  }

  public sum() {
    this.calculatorService.getCallSum().subscribe(data =>
    {
      this.calculatorService.result = data;
    });
  }

  public subtract() {
    this.calculatorService.getCallSubtract().subscribe(data => {
      this.calculatorService.result = data;
    });
  }

  public multiply() {
    this.calculatorService.getCallMultiply().subscribe(data => {
      this.calculatorService.result = data;
    });
  }

  public split() {
    this.calculatorService.getCallSplit().subscribe(data => {
      this.calculatorService.result = data;
    });
  }
}
