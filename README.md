
The web api will start in development mode in IIS express on https://localhost:44344

Available endpoints: 

 GET api/merchant/{merchantId}/payment/{paymentId}

 HTTP 200 - payment found <br />
 HTTP 404 - payment not found

 POST api/merchant/{merchantId}/payment <br />
 { <br />
   &nbsp; "CardNumber" : "", <br />
   &nbsp; "NameOnCard": "L S Smith", <br />
   &nbsp; "CardExpiryMonth": 12, <br />
   &nbsp; "CardExpiryYear": 2023, <br />
   &nbsp; "Amount": 23, <br />
   &nbsp; "Currency": "GBP", <br />
   &nbsp; "Cvv": "345" <br />
 } <br />

 HTTP 200 - payment fulfilled <br />
 HTTP 400 - expired card/ merchant not found/ invalid input fields

The external dependencies (Bank client & Data stores) are not implemented/configured in this solution 

## Assumptions
 
The payment will be a synchronous operation so the post endpoint will call the Acquiring Bank and wait for a response.

## Testing

1.The api endpoints have a set of in memory tests that test the api contracts and the communication between al application layers 
The testing is happening with in memory stubs for the external dependencies. <br />

2.The api has a set of unit level tests for domain models logic. 

3.Setup test environmnet before running the the api integration tests
- Install node and node package manager : https://nodejs.org/en/download/
- Install Mountebank : npm install -g mountebank, http://www.mbtest.org/docs/gettingStarted
- Start Mountebank : mb
- Install cosmos db emulator 
- Start the cosmos db emulator 

4.Test Http requests 

- Start the cosmos db emulator 
- Run integration tests to create test data
- Start mountebank using imposters.json from project root : mb restart --configfile imposters.json
- Start the PaymentGateway api in IIS Express
- Run below test http requests

POST https://localhost:44344/api/merchant/12345670/payment  HTTP/1.1
Content-Type: application/json
Host: localhost:44344
Content-Length: 160

{  "CardNumber" : "4111111111111111", "NameOnCard": "L S Smith", "CardExpiryMonth": 12, "CardExpiryYear": 2023, "Amount": 345,  "Currency": "GBP", "Cvv": "345"}


GET https://localhost:44344/api/merchant/12345670/payment/234  HTTP/1.1
Host: localhost:44344


## Areas for improvement

1.The payment operation should be implemented in multiple asynchronous steps: <br />
    - receive user payment request and raise a payment request event into a payment queue<br />
    - a listener will consume the payment queue and call the Acquiring Bank to make the payment & raise a payment complete event<br />
    - a listener will consume the payment complete event queue and update the payment repository<br />
Asynchronous payment process will be resilient to failures (events can be retried or replayed) and it is easier/less expensive to scale up <br />

2.More Acquiring Bank Clients can be added to the api by having multiple implementations to the IAcquiringBank interface <br />
  and select the required one by implementing a strategy pattern <br />

3.The converter extension methods can become an IConverter implementation to isolate the conversion logic so it can be unit tested independently<br />

4.Add different data store by changing the implementation of IPaymentRepository & IMerchantRepository<br />

5.Further uit tests can be added for better coverage (eg. unit test conversion logic or the Application Handlers using mocks)<br />

6.The domain models’ properties have public setters & public empty constructor for the cosmos persistence layer purpose. 
The Domain and Interface layers can be decoupled by adding data transfer objects for persistence concern only.
This data transfer objects can be converted to clean domain models.<br />

## Cloud technologies

1. I would go for AWS Lambda with Amazon API Gateway to start with and if traffic increases I will host the api in an EC2 box<br />
2. I would choose the SQS/SNS for asynchronous events<br />
3. DynamoDB for storing payments and merchants<br />

## Extra Mile

1. I aimed to use onion architecture to structure the api layers and build a payments bounded context based on the domain knowledge learned from the Payment Gateway exercise<br />

2. Also I was trying to avoid using exceptions for control flow, so I went for the result pattern approach using FluentResults package 


