entity some_test_bench is
end entity;

architecture some_test_bench of some_test_bench is
begin

	compute_xor: process
		variable b : integer := 0;
		variable c : integer := 10;
	begin
	  report(b);
	  report(c);
	  report(b<c);
	  report(b<=c);
	  report(b>c);
	  report(b>=c);
	  report(b=c);
	  report(b/=c);
	  
	  wait;
	end process;

end architecture some_test_bench;