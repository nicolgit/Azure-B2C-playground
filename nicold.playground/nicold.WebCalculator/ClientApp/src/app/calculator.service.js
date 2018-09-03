"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CalculatorService = /** @class */ (function () {
    function CalculatorService(http) {
        this.http = http;
        this.parameter1 = 12;
        this.parameter2 = 34;
        this.isAuthenticated = false;
        this.username = "prova";
        this.result = 0;
        this.applicationConfig = {
            clientID: 'c07391de-3205-4496-a704-4607b18b64f9',
            authority: "https://login.microsoftonline.com/tfp/nicolb2c.onmicrosoft.com/B2C_1_signin-default",
            b2cScopes: ["https://nicolb2c.onmicrosoft.com/WebCalculator/user_impersonation"],
            calculatorApiEndopoint: "https://nicolapicalculator.azurewebsites.net"
        };
    }
    CalculatorService.prototype.callSum = function () {
        return 5;
    };
    return CalculatorService;
}());
exports.CalculatorService = CalculatorService;
//# sourceMappingURL=calculator.service.js.map