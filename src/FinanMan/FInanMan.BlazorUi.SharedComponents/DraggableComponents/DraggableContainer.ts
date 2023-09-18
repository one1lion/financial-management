export function handleDragStart(event: DragEvent) {
    event.dataTransfer.setDragImage(new Image(), 0, 0);
}

export function capturePointerEvents(elem: HTMLElement) {
    elem.setPointerCapture(1);
}

export function releasePointerEvents(elem: HTMLElement) {
    elem.releasePointerCapture(1);
}