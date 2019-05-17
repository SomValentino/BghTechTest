import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { RouterModule } from '@angular/router';
import {NgxPaginationModule} from 'ngx-pagination';
import { ToastrModule } from 'ngx-toastr';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import { appRoutes } from './route';
import { HomeValidIdComponent } from './home-validId/home-validId.component';
import { HomeInvalidIdComponent } from './home-invalidId/home-invalidId.component';
import { HomeInputComponent } from './home-input/home-input.component';

@NgModule({
   declarations: [
      AppComponent,
      HomeComponent,
      HomeValidIdComponent,
      HomeInvalidIdComponent,
      HomeInputComponent
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      FormsModule,
      ReactiveFormsModule,
      HttpClientModule,
      RouterModule.forRoot(appRoutes),
      NgxPaginationModule,
      ToastrModule.forRoot()
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
