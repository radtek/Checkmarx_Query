/*
This query will point the Argon2PasswordEncoder instance that:

Use the default recommended parameters, otherwise follow the parameter selection recommendations defined by the authors of the algorithms.

When using Argon2 the following parameters are recommended:

Number of Lanes (h):

	- Double the number of CPUs

Amount of memory to be used (m):

	- Backend Authentication - 4GiB
	- Frontend Authentication - 1GiB
	- Hard-drive Encryption Key - 6GiB

Constructor and Description

	- Argon2PasswordEncoder() 
	- Argon2PasswordEncoder(int saltLength, int hashLength, int parallelism, int memory, int iterations)

Note:

The currently implementation uses Bouncy castle which does not exploit parallelism/optimizations that password crackers will,
 so there is an unnecessary asymmetry between attacker and defender.

*/

// Recommended Parameters

int saltLength = 16; // salt length in bytes
int hashLength = 16; // hash length in bytes
int parallelism = 1; // currently not supported by Spring Security
int memory = 1024;   // memory costs
int iterations = 3; // iterations

result = Spring_Find_Argon2_Insecure_Parameters(saltLength, hashLength, parallelism, memory, iterations);