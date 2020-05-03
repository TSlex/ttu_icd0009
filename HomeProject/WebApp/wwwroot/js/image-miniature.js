let PaddingTop = document.getElementById("PaddingTop");
let PaddingRight = document.getElementById("PaddingRight");
let PaddingBottom = document.getElementById("PaddingBottom");
let PaddingLeft = document.getElementById("PaddingLeft");
let WidthPx = document.getElementById("WidthPx");
let HeightPx = document.getElementById("HeightPx");

let fixRation = false;

document.addEventListener("keydown", ev => {
    if (ev.key === "Shift") {
        fixRation = true;
    }
});

document.addEventListener("keyup", ev => {
    if (ev.key === "Shift") {
        fixRation = false;
    }
});

let container = document.getElementById("image-miniature");

container.style.position = "relative";
console.log(container);

let cover = createElement("miniature-cover");

container.append(cover);
let coverWidth = cover.offsetWidth;
let coverHeight = cover.offsetHeight;

let box1 = createElement("miniature-control");
box1.style.left = PaddingLeft.value + 'px';
box1.style.top = PaddingTop.value + 'px';

let box2 = createElement("miniature-control");
box2.style.right = PaddingRight.value + 'px';
box2.style.top = PaddingTop.value + 'px';

let box3 = createElement("miniature-control");
box3.style.left = PaddingLeft.value + 'px';
box3.style.bottom = PaddingBottom.value + 'px';

let box4 = createElement("miniature-control");
box4.style.right = PaddingRight.value + 'px';
box4.style.bottom = PaddingBottom.value + 'px';

cover.style.marginTop = PaddingTop.value + 'px';
cover.style.marginRight = PaddingRight.value + 'px';
cover.style.marginBottom = PaddingBottom.value + 'px';
cover.style.marginLeft = PaddingLeft.value + 'px';

cover.style.width = WidthPx.value + 'px';
cover.style.height = HeightPx.value + 'px';

setResizeEvent(box1);
setResizeEvent(box2);
setResizeEvent(box3);
setResizeEvent(box4);

container.append(box1);
container.append(box2);
container.append(box3);
container.append(box4);

function setResizeEvent(item) {
    let callback = (e) => {
        resize(item, e)
    };

    item.addEventListener("mousedown", ev => {
        document.addEventListener("mousemove", callback)
    });

    document.addEventListener("mouseup", ev => {
        document.removeEventListener("mousemove", callback)
    });
}

function resize(item, event) {

    // console.log(event.x);
    // console.log(container.x);

    let offset = container.getBoundingClientRect();
    // console.log(fixRation);

    if (fixRation) {
        switch (item) {
            case box1:
                box1.style.top = getPosition(event, 'top', offset);
                box1.style.left = box1.style.top.valueOf();

                box2.style.top = box1.style.top.valueOf();
                box3.style.left = box1.style.top.valueOf();

                cover.style.marginLeft = parse(box1.style.left) + "px";
                cover.style.marginTop = parse(box1.style.top) + "px";
                break;
            case box2:
                box2.style.top = getPosition(event, 'top', offset);
                box2.style.right = box2.style.top.valueOf();

                box1.style.top = box2.style.top.valueOf();
                box4.style.right = box2.style.top.valueOf();

                cover.style.marginRight = parse(box2.style.right) + "px";
                cover.style.marginTop = parse(box2.style.top) + "px";
                break;
            case box3:
                box3.style.left = getPosition(event, 'bottom', offset);
                box3.style.bottom = box3.style.left.valueOf();

                box1.style.left = box3.style.left.valueOf();
                box4.style.bottom = box3.style.left.valueOf();

                cover.style.marginLeft = parse(box3.style.left) + "px";
                cover.style.marginBottom = parse(box3.style.bottom) + "px";

                break;
            case box4:
                box4.style.right = getPosition(event, 'bottom', offset);
                box4.style.bottom = box4.style.right.valueOf();

                box2.style.right = box4.style.right.valueOf();
                box3.style.bottom = box4.style.right.valueOf();

                cover.style.marginRight = parse(box4.style.right) + "px";
                cover.style.marginBottom = parse(box4.style.bottom) + "px";

                break;
        }
    } else {
        switch (item) {
            case box1:
                box1.style.top = getPosition(event, 'top', offset);
                box1.style.left = getPosition(event, 'left', offset);

                box2.style.top = getPosition(event, 'top', offset);
                box3.style.left = getPosition(event, 'left', offset);

                cover.style.marginLeft = parse(box1.style.left) + "px";
                cover.style.marginTop = parse(box1.style.top) + "px";
                break;
            case box2:
                box2.style.top = getPosition(event, 'top', offset);
                box2.style.right = getPosition(event, 'right', offset);

                box1.style.top = getPosition(event, 'top', offset);
                box4.style.right = getPosition(event, 'right', offset);

                cover.style.marginRight = parse(box2.style.right) + "px";
                cover.style.marginTop = parse(box2.style.top) + "px";
                break;
            case box3:
                box3.style.left = getPosition(event, 'left', offset);
                box3.style.bottom = getPosition(event, 'bottom', offset);

                box1.style.left = getPosition(event, 'left', offset);
                box4.style.bottom = getPosition(event, 'bottom', offset);

                cover.style.marginLeft = parse(box3.style.left) + "px";
                cover.style.marginBottom = parse(box3.style.bottom) + "px";

                break;
            case box4:
                box4.style.right = getPosition(event, 'right', offset);
                box4.style.bottom = getPosition(event, 'bottom', offset);

                box2.style.right = getPosition(event, 'right', offset);
                box3.style.bottom = getPosition(event, 'bottom', offset);

                cover.style.marginRight = parse(box4.style.right) + "px";
                cover.style.marginBottom = parse(box4.style.bottom) + "px";

                break;
        }
    }

    cover.style.width = coverWidth - parse(cover.style.marginLeft) - parse(cover.style.marginRight) + "px";
    cover.style.height = coverHeight - parse(cover.style.marginTop) - parse(cover.style.marginBottom) + "px";

    HeightPx.value = parse(cover.style.height);
    WidthPx.value = parse(cover.style.width);

    PaddingTop.value = parse(cover.style.marginTop);
    PaddingRight.value = parse(cover.style.marginRight);
    PaddingBottom.value = parse(cover.style.marginBottom);
    PaddingLeft.value = parse(cover.style.marginLeft);
}

function getPosition(event, position, offset) {
    switch (position) {
        case 'top':
            return isFit(event.y - offset.y - 5, coverHeight) + "px";
        case 'right':
            return isFit(offset.x + offset.width - event.x - 5, coverWidth) + "px";
        case 'bottom':
            return isFit(offset.y + offset.height - event.y - 5, coverHeight) + "px";
        case 'left':
            return isFit(event.x - offset.x - 5, coverWidth) + "px";
    }
}

function isFit(value, max) {
    if (value < 0) {
        return 0
    } else if (value > max) {
        return max
    }
    return value
}

function createElement(className, tagName, id) {
    let elem = document.createElement(tagName || "div");
    elem.className = className;

    if (id != null) {
        elem.id = id.toString();
    }

    return elem
}

function parse(value) {
    let val = parseInt(value);

    return isNaN(val) ? 0 : val;
}
