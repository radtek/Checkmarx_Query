/*
This query will point the SCryptPasswordEncoder instance that:

Use the default recommended parameters, otherwise follow the parameter selection recommendations defined by the authors of the algorithms.

When using scrypt the following values for its parameters are recommended:

CPU/Memory cost (N):
	2^14

Internal block size (r):
	8

Parallel lanes (p):
	1

Constructor and Description

	- SCryptPasswordEncoder() 
	- SCryptPasswordEncoder(int cpuCost, int memoryCost, int parallelization, int keyLength, int saltLength)

Note:

Parameters:

	- cpuCost - cpu cost of the algorithm (as defined in scrypt this is N). must be power of 2 greater than 1. 
		- Default is currently 16,348 or 2^14)

	- memoryCost - memory cost of the algorithm (as defined in scrypt this is r) 
		- Default is currently 8.

	- parallelization - the parallelization of the algorithm (as defined in scrypt this is p) 
		- Default is currently 1. Note that the implementation does not currently take advantage of parallelization.

	- keyLength - key length for the algorithm (as defined in scrypt this is dkLen).
		- The default is currently 32.

	- saltLength - salt length (as defined in scrypt this is the length of S).
		- The default is currently 64.

*/

// Recommended Parameters

int cpuCost = (int) 2^14; // factor to increase CPU costs
int memoryCost = 8;      // increases memory usage
int parallelization = 1; // currently not supported by Spring Security
int keyLength = 32;      // key length in bytes
int saltLength = 64;     // salt length in bytes

result = Spring_Find_SCrypt_Insecure_Parameters(cpuCost, memoryCost, parallelization, keyLength, saltLength);