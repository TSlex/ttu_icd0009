import { autoinject, bindable } from 'aurelia-framework';

@autoinject
export class LoadingOverlay {

    constructor() {
    }

    @bindable public fixed: boolean = true;
    @bindable public style!: string;

    get IsFixed() {
        return this.fixed === true;
    }

    get Style() {
        let style = "left: 0px; top: 0px; height: 100%; width: 100%; background: rgba(0, 0, 0, 0.133); z-index: 100;"

        if (this.IsFixed) {
            style = "position: fixed; " + style;
        } else { 
            style = "position: absolute; " + style;
        }

        if (this.style){
            style = style + this.style;
        }

        return style;
    }
}
