export function handleDragStart(event: DragEvent) {
    event.dataTransfer.setDragImage(new Image(), 0, 0);
}