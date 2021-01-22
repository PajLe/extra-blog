import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DataRest } from '../../shared/models/data-response';

@Injectable({
	providedIn: 'root'
})
export class DataService {

	constructor(private http: HttpClient) { }

	public registerUser(data: any): Observable<DataRest> {
		const path = 'http://localhost:5000/authentication/register'
		return this.http.post<DataRest>(path, data);
	}

	public login(user): Observable<DataRest> {
		const path = 'http://localhost:5000/authentication/login'
		return this.http.post<DataRest>(path, user);
	}

	public getAllDocuments(): Observable<DataRest> {
		const path = 'http://localhost:5000/api/document'
		return this.http.get<DataRest>(path);
	}

	public getNewsFeed(skip: number): Observable<any> {
		const path = 'http://localhost:5000/api/user/news/' + JSON.parse(localStorage.getItem('decoded')).unique_name + '/' + skip
		return this.http.get<DataRest>(path);
	}

	public getAllCategories() {
		const path = 'http://localhost:5000/api/category'
		return this.http.get<DataRest>(path);
	}
	
	public getMyDocuments() {
		const path = 'http://localhost:5000/api/document/my/' + JSON.parse(localStorage.getItem('decoded')).unique_name
		return this.http.get<DataRest>(path);
	}

	public saveCategory(data) {
		const path = 'http://localhost:5000/api/category/add/' + data.name
		return this.http.post<DataRest>(path, {});
	}

	public saveDocument(data) {
		data.pictures = data.pictures.split(';');
		data.paragraphs = data.paragraphs.split("\n");
		data.createdBy = JSON.parse(localStorage.getItem('decoded')).unique_name;
		const path = 'http://localhost:5000/api/document/add'
		return this.http.post<DataRest>(path, data);
	}

	public deleteDocument(data) {
		const path = 'http://localhost:5000/api/document/' + data
		return this.http.delete<DataRest>(path);
	}

	public editDocument(data) {
		data.pictures = data.pictures.split(';');
		data.paragraphs = data.paragraphs.split("\n")
		const path = 'http://localhost:5000/api/document/edit/' + data.name
		return this.http.put<DataRest>(path, data);
	}

	public addTagsDocument(data) {
		const path = 'http://localhost:5000/api/document/tags/' + data.documentName
		return this.http.put<DataRest>(path, data.tags);
	}

	public followCategory(data) {
		const path = 'http://localhost:5000/api/category/addinterest/' + data + '/' + JSON.parse(localStorage.getItem('decoded')).unique_name
		return this.http.put<DataRest>(path, {});
	}

	public getTrending(): Observable<DataRest> {
		const path = 'http://localhost:5000/api/category/trending'
		return this.http.get<DataRest>(path);
	}
		
	public seen(name: any) {
		const path = 'http://localhost:5000/api/document/addseen/' + name + '/' + JSON.parse(localStorage.getItem('decoded')).unique_name
		return this.http.put<DataRest>(path, {});
	  }
}
