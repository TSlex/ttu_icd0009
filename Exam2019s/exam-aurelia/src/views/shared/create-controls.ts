import { autoinject, bindable } from 'aurelia-framework';
import { EventAggregator, Subscription } from 'aurelia-event-aggregator';

@autoinject
export class CreateControls {

    constructor(private eventAggregator: EventAggregator) {
    }

    onSubmit() {
        this.eventAggregator.publish('onSubmit')
    }

    onCancel() {
        this.eventAggregator.publish('onCancel')
    }
}
