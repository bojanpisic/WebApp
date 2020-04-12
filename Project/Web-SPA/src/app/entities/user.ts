export class User {
    imageUrl: string;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    userType: string;
    id: number;
    phone: string;
    address: string;

    constructor(fname: string, lname: string, pasw: string, email: string, city: string, phone: string) {
        this.firstName = fname;
        this.lastName = lname;
        this.email = email;
        this.password = pasw;
        this.phone = phone;
        this.address = city;
    }
}
