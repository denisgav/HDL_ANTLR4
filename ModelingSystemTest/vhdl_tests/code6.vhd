entity some_test_bench is
end entity;

architecture some_test_bench of some_test_bench is
	variable b : integer := 2#1010#2;
	variable o : integer := 8#10#;
	variable h : integer := 16#FFF#;
begin

	compute_xor: process begin
	  report(b);
	  report(o);
	  report(h);

	  report(h+1);	  
	  wait;
	end process;
end architecture some_test_bench;