import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'trimTrailingZeros'
})
export class TrimTrailingZerosPipe implements PipeTransform {
  transform(value: number): string {
    if (value == null) return '';
    const rounded = value.toFixed(3);
    return parseFloat(rounded).toString();
  }
}
