let canvas = document.createElement('canvas');
let image = document.getElementById('render_image');
let _image = document.createElement('img');

_image.src = image.src;

_image.onload = () => {
    render()
};

render();

function render() {
    let paddingTop = image.dataset.pt;
    let paddingLeft = image.dataset.pl;
    let paddingBottom = image.dataset.pb;
    let paddingRight = image.dataset.pr;

    let originalHeight = image.dataset.oheight;
    let originalWidth = image.dataset.owidth;

    canvas.width = originalWidth - paddingRight - paddingLeft;
    canvas.height = originalHeight - paddingTop - paddingBottom;

    let ctx = canvas.getContext('2d');
    ctx.drawImage(_image, paddingLeft, paddingTop, canvas.width, canvas.height, 0, 0, canvas.width, canvas.height);
    image.src = canvas.toDataURL();
    image.style.display = 'initial';
}