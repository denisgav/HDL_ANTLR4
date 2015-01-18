entity some_test_bench is
end entity;

architecture some_test_bench of some_test_bench is
	variable b : integer := 0;
	variable c : integer := 10;
begin

	compute_xor: process
	begin
	  report(b);
	  report(c);
	  IF    b > c THEN
		report(100);
	  ELSIF b = c THEN
	    report(200);
	  ELSE
		report(300);
	  END IF;
	  
	  wait;
	end process;

end architecture some_test_bench;