function Button({ value, className, href, target, onClick }) {
  return (
    <a
      href={href ? href : "#"}
      target={target ? target : "_self"}
      className={`flex justify-center items-center rounded px-4 py-2 text-neutral-800 ${className}`}
      onClick={onClick}
    >
      <span className="font-bold">{value}</span>
    </a>
  );
}

export default Button;
