import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';

@Component({
  selector: 'app-infinite-scroll',
  template: `<ng-content></ng-content><div #anchor></div>`
})
export class InfiniteScrollComponent implements OnInit, OnDestroy, AfterViewInit {

  @Input() options = {};
  @Output() scrolled = new EventEmitter();
  @ViewChild('anchor') anchor: ElementRef<HTMLElement>;

  private observer: IntersectionObserver;

  constructor(private host: ElementRef) { }

  ngAfterViewInit(): void {
    this.observer.observe(this.anchor.nativeElement);
  }

  ngOnInit() {
    const options = {
      root: this.isHostScrollable() ? this.host.nativeElement : null,
      ...this.options
    };

    this.observer = new IntersectionObserver(([entry]) => {
      entry.isIntersecting && this.scrolled.emit();
    }, options);
  }

  private isHostScrollable() {
    const style = window.getComputedStyle(this.element);

    return style.getPropertyValue('overflow') === 'auto' ||
      style.getPropertyValue('overflow-y') === 'scroll';
  }
  
  ngOnDestroy() {
    this.observer.disconnect();
  }

  get element() {
    return this.host.nativeElement;
  }
}
