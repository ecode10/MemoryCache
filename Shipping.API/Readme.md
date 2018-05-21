# Farfetch Coding Challenge - Shipping API

## Description

     In this solution you'll find a basic implementation of a REST API that will serve as a proxy to some shippers. For the 
     sake of simplicity, we will only consider two of them: DHL and UPS. 
     
     The Shipping API has only one resource for now and its job is to retrieve the scheduled pickups for a given carrier.
    
     The implementation is following the requirements below:
    
     #1 - The Shipping API must expose an endpoint that allows us to get a list of booked pickups for a given shipper id.
    
	#2 - When the shipper is DHL then:
		#2.1 - the DHL pickups must be retrieved from cache when there are cached values.
		#2.2 - the DHL pickups must be retrieved from the DHL service when the cache is empty or turned off.
		#2.3 - The cache must be optional, so that it can be turned off/on based on some configuration loaded at the startup.
    
	#3 - When shipper selected is UPS, then the pickups must be always retrieved from the UPS service.
    
	Please be aware that the current implementation is not following any of the best practices, and the goal of this exercise 
	is for you to refactor it without changing any of the requirements until you're happy with it.
		 
	Good luck!