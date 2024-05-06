function Button({ value, className, url }) {
  return (
    <a
      href={url ? url : "#"}
      target={"_blank" ? url : "_self"}
      className={`flex justify-center items-center rounded px-4 py-2 text-neutral-800 ${className}`}
    >
      <span className="font-bold">{value}</span>
    </a>
  );
}

export default Button;
