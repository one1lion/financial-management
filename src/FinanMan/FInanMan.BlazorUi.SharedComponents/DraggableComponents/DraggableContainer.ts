export function handleDragStart(event: DragEvent) {
    event.dataTransfer.setDragImage(new Image(), 0, 0);
}

export function capturePointerEvents(elem: HTMLElement) {
    console.log("Pointer captures on", elem);
    elem.setPointerCapture(1);
}

export function releasePointerEvents(elem: HTMLElement) {
    console.log("Pointer released on", elem);
    elem.releasePointerCapture(1);
}