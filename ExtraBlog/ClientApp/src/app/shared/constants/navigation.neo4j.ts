import { Navigation } from "../models/navigation";

export class NavigationNeo extends Navigation {
    static Home = new Navigation('home');
    
    static User = new Navigation('user');

    static Explore = new Navigation('user/explore');

    static Network = new Navigation('user/network');
}