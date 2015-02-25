entity some_test_bench is
end entity;

architecture some_test_bench of some_test_bench is
	variable time1 : TIME := 5 ps;
	variable time2 : TIME := 3 ns;
begin

	compute_xor: process begin
		report(5 fs + 8 fs) ;
		report(time1) ;
		report(time1 + 8 fs) ;
		report(time2) ;
		report(time2 - 8 fs) ;
		report(time1 + time2) ;
		report(time1 / 2) ;
		report(time2 / 2) ;
		report(time2 * 3) ;
	  wait;
	end process;
end architecture some_test_bench;