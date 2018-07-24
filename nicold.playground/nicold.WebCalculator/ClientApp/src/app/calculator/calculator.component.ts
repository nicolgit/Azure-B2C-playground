import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CalculatorService } from '../calculator.service';

@Component({
  selector: 'app-counter-component',
  templateUrl: './calculator.component.html',
  providers: [CalculatorService],
})

export class CalculatorComponent {

  constructor(private calculatorService: CalculatorService) {
    
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

  get result(): string {
    if (this.calculatorService.isAuthenticated) {
      return this.calculatorService.result.toString();
    }
    else {
      return "";
    }
  }

  public authenticate() {
    
  }

  
}
