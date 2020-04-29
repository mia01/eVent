import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import Event from 'ClientApp/app/models/event';
import IAddEventForm from 'ClientApp/app/interfaces/addEventForm';

@Component({
  selector: 'app-add-event',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddEventComponent implements OnInit {

  form: FormGroup;
  event: Event;

  constructor(
    private formBuilder: FormBuilder,
    private modal: MatDialogRef<AddEventComponent>,
    @Inject(MAT_DIALOG_DATA) private data: IAddEventForm
  ) {
    this.event = data.eventData;
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      id: this.event ? this.event.id : null,
      title: [this.event ? this.event.title : "", Validators.required],
      description: this.event ? this.event.description : "",
      startDate: [this.event ? new Date(this.event.startDate + "Z") : "", Validators.required],
      endDate: [this.event ? new Date(this.event.endDate + "Z") : "", Validators.required],
      reminder: [this.event ? this.event.reminder : false],
      publicEvent: [this.event ? this.event.publicEvent : false],
    })
  }

  submit(form) {
    if (form.valid) {
      var event = form.value as Event;
      if (event.reminder == null) {
        event.reminder = false;
      }

      if (event.publicEvent == null) {
        event.publicEvent = false;
      }
      this.modal.close(event);
    }
  }
}
