"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CalculatorService = /** @class */ (function () {
    function CalculatorService() {
        this.parameter1 = 12;
        this.parameter2 = 34;
        this.isAuthenticated = false;
        this.username = "prova";
        this.applicationConfig = {
            clientID: 'c07391de-3205-4496-a704-4607b18b64f9',
            authority: "https://login.microsoftonline.com/tfp/nicolb2c.onmicrosoft.com/B2C_1_signin-default",
            b2cScopes: ["https://nicolb2c.onmicrosoft.com/WebCalculator/user_impersonation"],
        };
    }
    return CalculatorService;
}());
exports.CalculatorService = CalculatorService;
//# sourceMappingURL=calculator.service.js.map