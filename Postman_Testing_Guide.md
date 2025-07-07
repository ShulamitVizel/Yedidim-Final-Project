# Yedidim API Testing Guide with Postman

## Overview
This guide will help you test your Yedidim backend API using Postman. The API is a volunteer management system with three main entities: Clients, Volunteers, and Calls.

## Setup Instructions

### 1. Start Your Backend Server
First, make sure your backend server is running:
```bash
cd Server
dotnet run
```
Your server should start on `https://localhost:7145`

### 2. Import Postman Collection
1. Open Postman
2. Click "Import" button
3. Select the `Yedidim_API_Tests.postman_collection.json` file
4. The collection will be imported with all test requests

## API Endpoints Overview

### Base URL
`https://localhost:7145/api`

### Available Controllers:
- **Clients**: `/client`
- **Volunteers**: `/volunteer` 
- **Calls**: `/call`

## Testing Strategy

### Phase 1: Basic CRUD Operations
Test the fundamental Create, Read, Update, Delete operations for each entity.

### Phase 2: Business Logic
Test the advanced features like volunteer assignment, ETA calculation, and matching algorithms.

### Phase 3: Error Handling
Test edge cases and error scenarios.

## Detailed Test Scenarios

### Clients Testing

#### 1. Create Client
- **Request**: POST `/api/client`
- **Body**:
```json
{
  "name": "John Doe",
  "phoneNumber": "+972-50-123-4567"
}
```
- **Expected Response**: 201 Created with the created client data
- **Test**: Verify the client is created with an auto-generated ID

#### 2. Get All Clients
- **Request**: GET `/api/client`
- **Expected Response**: 200 OK with array of clients
- **Test**: Verify the newly created client appears in the list

#### 3. Get Client by ID
- **Request**: GET `/api/client/{id}`
- **Expected Response**: 200 OK with client data or 404 Not Found
- **Test**: Try with existing and non-existing IDs

#### 4. Delete Client
- **Request**: DELETE `/api/client/{id}`
- **Expected Response**: 204 No Content
- **Test**: Verify the client is removed from the system

### Volunteers Testing

#### 1. Create Volunteer
- **Request**: POST `/api/volunteer`
- **Body**:
```json
{
  "name": "Sarah Cohen",
  "level": "Advanced",
  "isAvailable": true,
  "phoneNumber": "+972-52-987-6543",
  "volunteerLatitude": 32.0853,
  "volunteerLongitude": 34.7818
}
```
- **Expected Response**: 201 Created
- **Test**: Verify all fields are saved correctly

#### 2. Get Available Volunteers
- **Request**: GET `/api/volunteer/available`
- **Expected Response**: 200 OK with only available volunteers
- **Test**: Verify only volunteers with `isAvailable: true` are returned

#### 3. Update Volunteer
- **Request**: PUT `/api/volunteer/{id}`
- **Body**: Updated volunteer data
- **Expected Response**: 204 No Content
- **Test**: Verify changes are persisted

### Calls Testing

#### 1. Create Call
- **Request**: POST `/api/call`
- **Body**:
```json
{
  "callTime": "2024-01-15T10:30:00",
  "clientId": 1,
  "finalVolunteerId": 0,
  "callType": "Emergency",
  "callLatitude": 32.0853,
  "callLongitude": 34.7818
}
```
- **Expected Response**: 201 Created
- **Test**: Verify call is created with proper relationships

#### 2. Assign Volunteer to Call
- **Request**: POST `/api/call/{callId}/volunteers/{volunteerId}`
- **Expected Response**: 204 No Content
- **Test**: Verify volunteer is assigned to the call

#### 3. Get Matching Volunteers
- **Request**: GET `/api/call/{callId}/matching-volunteers`
- **Expected Response**: 200 OK with matching volunteers
- **Test**: Verify algorithm returns appropriate volunteers

#### 4. Assign Best Volunteer
- **Request**: POST `/api/call/{callId}/assign-best`
- **Expected Response**: 200 OK with success message
- **Test**: Verify the nearest available volunteer is assigned

#### 5. Get ETA
- **Request**: GET `/api/call/{callId}/eta?volunteerId={volunteerId}`
- **Expected Response**: 200 OK with estimated arrival time
- **Test**: Verify ETA calculation works correctly

## Test Data Setup

### Recommended Test Sequence:
1. **Create test data**:
   - Create 2-3 clients
   - Create 3-4 volunteers with different locations and availability
   - Create 1-2 calls

2. **Test relationships**:
   - Assign volunteers to calls
   - Test the matching algorithm
   - Test ETA calculations

3. **Test edge cases**:
   - Try to assign unavailable volunteers
   - Test with non-existent IDs
   - Test with invalid data

## Expected HTTP Status Codes

- **200 OK**: Successful GET requests
- **201 Created**: Successful POST requests
- **204 No Content**: Successful PUT/DELETE requests
- **400 Bad Request**: Invalid data or ID mismatch
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server errors

## Common Issues and Solutions

### 1. SSL Certificate Issues
If you get SSL certificate errors:
- In Postman, go to Settings → General
- Turn off "SSL certificate verification"

### 2. Database Connection Issues
- Ensure your database is running
- Check connection string in `appsettings.json`
- Verify database migrations are applied

### 3. Google Maps API Issues
- The ETA calculation requires Google Maps API
- Ensure you have valid API keys configured
- Check if the service is properly configured

## Advanced Testing Tips

### 1. Use Environment Variables
Create a Postman environment with:
- `baseUrl`: `https://localhost:7145`
- `clientId`: Store created client IDs
- `volunteerId`: Store created volunteer IDs
- `callId`: Store created call IDs

### 2. Add Test Scripts
Add JavaScript tests to verify responses:
```javascript
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Response has required fields", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData).to.have.property('name');
    pm.expect(jsonData).to.have.property('phoneNumber');
});
```

### 3. Chain Requests
Use the response from one request as input for the next:
- Create a client, then use its ID to create a call
- Create a volunteer, then assign it to a call

## Performance Testing

### Load Testing Scenarios:
1. **Concurrent volunteer assignments**: Test multiple calls being assigned simultaneously
2. **Large dataset**: Test with many volunteers and calls
3. **Geographic distribution**: Test with volunteers spread across different locations

## Security Testing

### Test Cases:
1. **Input validation**: Try sending malformed JSON
2. **SQL injection**: Test with special characters in input
3. **Authorization**: Test if endpoints require authentication (if implemented)

## Monitoring and Logging

### Check Server Logs:
- Monitor the console output while running tests
- Look for any error messages or exceptions
- Verify database operations are logged

### Database Verification:
- Use SQL Server Management Studio to verify data persistence
- Check if relationships are properly maintained
- Verify data integrity constraints

## Next Steps

After completing these tests:
1. **Document any issues** found during testing
2. **Optimize performance** based on test results
3. **Add more test cases** for edge scenarios
4. **Implement automated testing** using tools like xUnit or NUnit
5. **Set up CI/CD pipeline** for continuous testing

## Support

If you encounter issues:
1. Check the server console for error messages
2. Verify database connectivity
3. Ensure all dependencies are properly configured
4. Review the API documentation in Swagger UI at `https://localhost:7145/swagger` 